using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using TMPro;
using UnityEngine;

public class LobbyManagerUI : MonoBehaviour
{
	public GameObject inLobbyPage;
	public GameObject lobbyPage;
	public GameObject lobbyPrefabParent;
	public GameObject lobbyPrefab;
	public UIPageSwitcher pageSwitcher;

	private MapManager mapManager;
	private LobbyManager lobbyManager;
	private InLobbyManagerUI inLobbyManagerUI;
	private AlertManager alertManager;
	private PlayerManager playerManager;

	public float lobbyRefreshTime = 2;
	private float lobbyRefreshTimeElapsed;

	public float emptyAndIdleRemoveTime = 5;
	private float emptyAndIdleRemoveTimeElapsed = 5;

	private Coroutine co_RemoveIdleAndEmptyLobbies;
	private Coroutine co_RefreshLobbies;
	private Coroutine co_CreateNewLobby;
	private Dictionary<string, Coroutine> co_UpdateLobbyUI = new Dictionary<string, Coroutine>();
	private Dictionary<string, Coroutine> co_AddLobbyUI = new Dictionary<string, Coroutine>();

	void Start()
	{
		UIManager uiManager = UIManager.GetInstance();
		GameManager gameManager = GameManager.GetInstance();
		lobbyManager = gameManager.dataManager.lobbyManager;
		inLobbyManagerUI = uiManager.inLobbyManagerUI;
		mapManager = gameManager.dataManager.mapManager;
		alertManager = uiManager.alertManager;
		playerManager = gameManager.dataManager.playerManager;

		lobbyRefreshTimeElapsed = lobbyRefreshTime;
		lobbyManager.OnLobbiesChanged += LobbiesChanged;
	}

	private void FixedUpdate()
	{
		if (lobbyPage.activeInHierarchy)
		{
			if (lobbyRefreshTimeElapsed > lobbyRefreshTime)
			{
				RefreshLobbies();
			}
			else
			{
				lobbyRefreshTimeElapsed += Time.deltaTime;
			}

			if (emptyAndIdleRemoveTimeElapsed > emptyAndIdleRemoveTime)
			{
				if (co_RemoveIdleAndEmptyLobbies != null) StopCoroutine(co_RemoveIdleAndEmptyLobbies);
				co_RemoveIdleAndEmptyLobbies = StartCoroutine(RemoveIdleAndEmptyLobbies());

				emptyAndIdleRemoveTimeElapsed = 0;
			}
			else
			{
				emptyAndIdleRemoveTimeElapsed += Time.deltaTime;
			}
		}
	}

	private IEnumerator RemoveIdleAndEmptyLobbies()
	{
		Debug.Log("Removing idle players and empty lobbies");
		yield return lobbyManager.FindAndRemoveInactivePlayers();
		yield return lobbyManager.RemoveEmptyLobbies();
	}

	public void ButtonCreateNewLobbyClicked()
	{
		if (co_CreateNewLobby != null) StopCoroutine(co_CreateNewLobby);
		co_CreateNewLobby = StartCoroutine(CreateNewLobby());
	}

	public IEnumerator CreateNewLobby()
	{
		alertManager.ShowLoadingAlert("Creating new lobby ...");

		Lobby lobby = null;
		yield return lobbyManager.CreateLobby((dblobby) =>
		{
			lobby = dblobby;
		});

		yield return OpenLobbyUI(lobby.Id);

		alertManager.CloseLoadingAlert();
	}

	public IEnumerator OpenLobbyUI(string lobbyId)
	{
		yield return lobbyManager.RefreshLobbies();

		LobbyStatusMessage statusMessage = LobbyStatusMessage.Deleted;
		yield return lobbyManager.CheckIfJoinable(playerManager.Client.Id, lobbyId, (message) => statusMessage = message);

		switch (statusMessage)
		{
			case LobbyStatusMessage.Full:
				alertManager.ShowMessageAlert("Could not join lobby", "The lobby you tried to join is full", "That sucks!");
				yield break;
			case LobbyStatusMessage.Started:
				alertManager.ShowMessageAlert("Could not join lobby", "The lobby you tried to join has already started", "Oh man!");
				yield break;
			case LobbyStatusMessage.Deleted:
				alertManager.ShowMessageAlert("Could not join lobby", "The lobby you tried to join does not exist anymore", "Yikes");
				yield break;
		}

		inLobbyManagerUI.CurrentLobbyId = lobbyId;

		yield return lobbyManager.JoinLobby(lobbyId);

		yield return inLobbyManagerUI.ChangePlayerColor();

		pageSwitcher.SwitchPage(inLobbyPage);
	}

	public void RefreshLobbies()
	{
		Debug.Log("Updating Lobbies from LobbyUI");
		if (co_RefreshLobbies != null) StopCoroutine(co_RefreshLobbies);
		co_RefreshLobbies = StartCoroutine(lobbyManager.RefreshLobbies());
		lobbyRefreshTimeElapsed = 0;
	}

	private void LobbiesChanged(object sender, LobbiesChangedEventArgs lobbiesChangedArgs)
	{
		foreach (var lobbyChange in lobbiesChangedArgs.ChangedLobbies)
		{
			switch (lobbyChange.Value)
			{
				case LobbyChangeState.New:
					if (co_AddLobbyUI.ContainsKey(lobbyChange.Key))
					{
						StopCoroutine(co_AddLobbyUI[lobbyChange.Key]);
						co_AddLobbyUI.Remove(lobbyChange.Key);
					}
					co_AddLobbyUI.Add(lobbyChange.Key, StartCoroutine(AddLobbyToUI(lobbyChange.Key)));
					break;
				case LobbyChangeState.Changed:
					if (co_UpdateLobbyUI.ContainsKey(lobbyChange.Key))
					{
						StopCoroutine(co_UpdateLobbyUI[lobbyChange.Key]);
						co_UpdateLobbyUI.Remove(lobbyChange.Key);
					}
					co_UpdateLobbyUI.Add(lobbyChange.Key, StartCoroutine(UpdateLobbyUI(lobbyChange.Key)));
					break;
				case LobbyChangeState.Deleted:
					RemoveLobbyFromUI(lobbyChange.Key);
					break;
			}
		}
	}

	private void RemoveLobbyFromUI(string lobbyId)
	{
		foreach (Transform transform in lobbyPrefabParent.transform)
		{
			if (transform.GetComponent<LobbyClick>().LobbyId.Equals(lobbyId))
			{
				Destroy(transform.gameObject);
				return;
			}
		}
	}

	private IEnumerator UpdateLobbyUI(string lobbyId)
	{
		GameObject existingLobby = FindLobbyCardUI(lobbyId);

		yield return UpdateLobbyUIObject(existingLobby, lobbyId);
	}

	private IEnumerator AddLobbyToUI(string lobbyId)
	{
		GameObject newLobby = Instantiate(lobbyPrefab, lobbyPrefabParent.transform);

		newLobby.GetComponent<LobbyClick>().LobbyId = lobbyId;

		yield return UpdateLobbyUIObject(newLobby, lobbyId);
	}

	private IEnumerator UpdateLobbyUIObject(GameObject lobbyObject, string lobbyId)
	{
		Lobby lobby = null;
		yield return lobbyManager.GetLobby(lobbyId, (dblobby) =>
		{
			lobby = dblobby;
		});

		if (lobby == null)
			yield break;

		KlootnMap lobbyMap = mapManager.GetMap(lobby.MapId);

		List<Player> lobbyPlayers = new List<Player>();
		yield return lobbyManager.GetLobbyPlayers(lobby.Id, (players) =>
		{
			lobbyPlayers = players;
		});

		int amountOfPlayer =  lobbyPlayers == null ? 0 : lobbyPlayers.Count;

		lobbyObject.transform.Find("LobbyTitle").GetComponent<TMP_Text>().text = lobby.Name;
		lobbyObject.transform.Find("LobbyDescription").GetComponent<TMP_Text>().text = lobby.Description;
		lobbyObject.transform.Find("LobbyPlayers").GetComponent<TMP_Text>().text = $"{amountOfPlayer}/{lobbyMap.maxPlayers}";
	}



	private GameObject FindLobbyCardUI(string id)
	{
		foreach (LobbyClick click in lobbyPrefabParent.GetComponentsInChildren<LobbyClick>())
		{
			if (click.LobbyId.Equals(id)) return click.gameObject;
		}
		return null;
	}

}
