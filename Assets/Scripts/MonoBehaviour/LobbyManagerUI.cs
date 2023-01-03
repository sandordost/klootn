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

			if(emptyAndIdleRemoveTimeElapsed > emptyAndIdleRemoveTime)
			{				
				RemoveIdleAndEmptyLobbies();
				emptyAndIdleRemoveTimeElapsed = 0;
			}
			else
			{
				emptyAndIdleRemoveTimeElapsed += Time.deltaTime;
			}
		}
	}

	private async void RemoveIdleAndEmptyLobbies()
	{
		Debug.Log("Removing idle players and empty lobbies");
		await lobbyManager.FindAndRemoveInactivePlayers();
		lobbyManager.RemoveEmptyLobbies();
	}

	public async void CreateNewLobby()
	{
		alertManager.ShowLoadingAlert("Creating new lobby ...");
		Lobby lobby = await lobbyManager.CreateLobby();
		OpenLobbyUI(lobby.Id);
	}

	public async void OpenLobbyUI(string lobbyId)
	{
		lobbyManager.RefreshLobbies();

		LobbyStatusMessage statusMessage = await lobbyManager.CheckIfJoinable(playerManager.Client.Id, lobbyId);

		switch (statusMessage)
		{
			case LobbyStatusMessage.Full:
				alertManager.ShowMessageAlert("Could not join lobby", "The lobby you tried to join is full", "That sucks!");
				return;
			case LobbyStatusMessage.Started:
				alertManager.ShowMessageAlert("Could not join lobby", "The lobby you tried to join has already started", "Oh man!");
				return;
			case LobbyStatusMessage.Deleted:
				alertManager.ShowMessageAlert("Could not join lobby", "The lobby you tried to join does not exist anymore", "Yikes");
				return;
		}

		inLobbyManagerUI.CurrentLobbyId = lobbyId;

		lobbyManager.JoinLobby(lobbyId);

		inLobbyManagerUI.ChangePlayerColor();

		pageSwitcher.SwitchPage(inLobbyPage);
	}

	public void RefreshLobbies()
	{
		Debug.Log("Updating Lobbies from LobbyUI");
		lobbyManager.RefreshLobbies();
		lobbyRefreshTimeElapsed = 0;
	}

	private void LobbiesChanged(object sender, LobbiesChangedEventArgs lobbiesChangedArgs)
	{
		foreach (var lobbyChange in lobbiesChangedArgs.ChangedLobbies)
		{
			switch (lobbyChange.Value)
			{
				case LobbyChangeState.New:
					AddLobbyToUI(lobbyChange.Key);
					break;
				case LobbyChangeState.Changed:
					UpdateLobbyUI(lobbyChange.Key);
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

	private void UpdateLobbyUI(string lobbyId)
	{
		GameObject existingLobby = FindLobbyCardUI(lobbyId);

		UpdateLobbyUIObject(existingLobby, lobbyId);
	}

	private void AddLobbyToUI(string lobbyId)
	{ 
		GameObject newLobby = Instantiate(lobbyPrefab, lobbyPrefabParent.transform);

		newLobby.GetComponent<LobbyClick>().LobbyId = lobbyId;

		UpdateLobbyUIObject(newLobby, lobbyId);
	}

	private async void UpdateLobbyUIObject(GameObject lobbyObject, string lobbyId)
	{
		Lobby lobby = await lobbyManager.GetLobby(lobbyId);

		KlootnMap lobbyMap = mapManager.GetMap(lobby.MapId);

		lobbyObject.transform.Find("LobbyTitle").GetComponent<TMP_Text>().text = lobby.Name;
		lobbyObject.transform.Find("LobbyDescription").GetComponent<TMP_Text>().text = lobby.Description;
		lobbyObject.transform.Find("LobbyPlayers").GetComponent<TMP_Text>().text = $"{lobby.Players.Count}/{lobbyMap.maxPlayers}";
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
