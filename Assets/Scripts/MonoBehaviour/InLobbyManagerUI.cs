using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InLobbyManagerUI : MonoBehaviour
{
	public TMP_Text titleText;
	public GameObject inLobbyPage;
	public GameObject playerUIPrefabParent;
	public GameObject playerUIPrefab;
	public GameObject mapGameObj;
	public GameObject mapChangeButton;

	private MapManager mapManager;
	private LobbyManager lobbyManager;
	private PlayerManager playerManager;
	private UIPageSwitcher uiPageSwitcher;
	private AlertManager alertManager;

	public float inLobbyRefreshTime = 3;
	private float inLobbyRefreshTimeElapsed = 0;

	private string currentLobbyId;

	public bool ClientIsHost { get; set; }

	public string CurrentLobbyId
	{
		get
		{
			return currentLobbyId;
		}
		set
		{
			currentLobbyId = value;
			CurrentPlayers.Clear();
			ClearPlayerListUI();
			UpdateInLobbyUI();
		}
	}

	private List<Player> currentPlayers;
	private List<Player> CurrentPlayers
	{
		get
		{
			if (currentPlayers is null)
			{
				currentPlayers = new List<Player>();
			}
			return currentPlayers;
		}
		set
		{
			currentPlayers = value;
		}
	}

	private Dictionary<string, PlayerColor> currentPlayerColors;
	private Dictionary<string, PlayerColor> CurrentPlayerColors
	{
		get
		{
			if (currentPlayerColors == null)
			{
				currentPlayerColors = new Dictionary<string, PlayerColor>();
			}
			return currentPlayerColors;
		}
		set
		{
			currentPlayerColors = value;
		}
	}

	private Coroutine refreshAndUpdateCoroutine;

	private void Start()
	{
		GameManager gameManager = GameManager.GetInstance();
		lobbyManager = gameManager.dataManager.lobbyManager;
		playerManager = gameManager.dataManager.playerManager;
		mapManager = gameManager.dataManager.mapManager;

		UIManager uiManager = UIManager.GetInstance();
		uiPageSwitcher = uiManager.uiPageSwitcher;
		alertManager = uiManager.alertManager;

		inLobbyRefreshTimeElapsed = inLobbyRefreshTime;

		lobbyManager.OnLobbiesChanged += LobbiesChanged;
	}

	private void ClearPlayerListUI()
	{
		foreach (Transform t in playerUIPrefabParent.transform)
		{
			Destroy(t.gameObject);
		}
	}

	private void LobbiesChanged(object sender, LobbiesChangedEventArgs e)
	{
		if (CurrentLobbyId != null && e.ChangedLobbies.ContainsKey(CurrentLobbyId))
		{
			if (e.ChangedLobbies[CurrentLobbyId].Equals(LobbyChangeState.Deleted))
				uiPageSwitcher.SwitchPage("LobbyPage");
			else UpdateInLobbyUI();
		}
	}

	private void FixedUpdate()
	{
		if (inLobbyPage.activeInHierarchy)
		{
			if (inLobbyRefreshTimeElapsed > inLobbyRefreshTime)
			{
				if (refreshAndUpdateCoroutine != null) StopCoroutine(refreshAndUpdateCoroutine);
				refreshAndUpdateCoroutine = StartCoroutine(RefreshAndUpdate());
			}
			else
			{
				inLobbyRefreshTimeElapsed += Time.deltaTime;
			}
		}
	}

	private IEnumerator RefreshAndUpdate()
	{
		Debug.Log("Updating Lobbies from inLobbyUI");
		yield return lobbyManager.RefreshLobbies();
		yield return lobbyManager.UpdateLobbyLastSeen(playerManager.Client.Id, CurrentLobbyId, Timestamp.GetCurrentTimestamp());
		yield return UpdatePlayerListUI();
		inLobbyRefreshTimeElapsed = 0;
	}

	private IEnumerator UpdateInLobbyUI()
	{
		alertManager.ShowLoadingAlert("Loading lobby ...");

		Lobby lobby = null;
		yield return lobbyManager.GetLobby(CurrentLobbyId, (dbLobby) => lobby = dbLobby);

		if (lobby is null)
		{
			alertManager.CloseLoadingAlert();
			uiPageSwitcher.SwitchPage("LobbyPage");
			yield break;
		}

		yield return lobbyManager.ClientIsHost(lobby.Id, (isHost) => ClientIsHost = isHost);

		KlootnMap map = null;
		yield return mapManager.GetCurrentKlootnMap(CurrentLobbyId, (klootnmap) => map = klootnmap);

		mapGameObj.transform.Find("Image").GetComponent<Image>().sprite = map.image;

		mapGameObj.transform.Find("MapInfo").Find("MapName").GetComponent<TMP_Text>().text = map.title;

		mapGameObj.transform.Find("MapInfo").Find("MapDescription").GetComponent<TMP_Text>().text = map.description;

		titleText.text = lobby.Name;

		UpdateHostControls(ClientIsHost);

		alertManager.CloseLoadingAlert();
	}

	private IEnumerator UpdateHostControls(bool clientIsHost)
	{
		Transform parent = playerUIPrefabParent.transform;

		mapChangeButton.SetActive(clientIsHost);

		string currentHost = null;
		yield return lobbyManager.GetHost(CurrentLobbyId, (newhost) => currentHost = newhost);

		foreach (Player_UI playerUI in parent.GetComponentsInChildren<Player_UI>())
		{
			GameObject playerCard = playerUI.gameObject;

			playerCard.transform.Find("NameAndIcon").Find("HostIcon").gameObject.SetActive(currentHost.Equals(playerUI.PlayerId));

			playerCard.transform.Find("KickAndPromote").gameObject.SetActive(clientIsHost && playerUI.PlayerId != playerManager.Client.Id);
		}
	}

	private IEnumerator UpdatePlayerListUI()
	{
		//Change Player area
		List<Player> lobbyPlayers = null;
		yield return lobbyManager.GetLobbyPlayers(CurrentLobbyId, (players) =>
		{
			lobbyPlayers = players;
		});

		Dictionary<string, PlayerColor> newColors = null;
		yield return lobbyManager.GetPlayerColors(CurrentLobbyId, (newplayercolors) =>
		{
			newColors = newplayercolors;
		});

		Dictionary<string, LobbyChangeState> lobbyPlayerChanges = lobbyManager.GetLobbyPlayersChanges(CurrentPlayers, lobbyPlayers, CurrentPlayerColors, newColors);

		foreach (var playerChange in lobbyPlayerChanges)
		{
			switch (playerChange.Value)
			{
				case LobbyChangeState.New:
					AddPlayerToLobbyUI(playerChange.Key);
					break;
				case LobbyChangeState.Changed:
					UpdatePlayerInLobbyUI(playerChange.Key);
					break;
				case LobbyChangeState.Deleted:
					RemovePlayerFromLobbyUI(playerChange.Key);
					break;
			}
		}

		CurrentPlayerColors = newColors;
		CurrentPlayers = lobbyPlayers;
	}

	public void KickPlayer(string playerId)
	{
		lobbyManager.KickPlayer(CurrentLobbyId, playerId);
	}

	private void RemovePlayerFromLobbyUI(string playerId)
	{
		if (playerManager.Client.Id.Equals(playerId))
		{
			uiPageSwitcher.SwitchPage("LobbyPage");
			alertManager.ShowMessageAlert("Removed from the lobby", "You have been removed from the lobby by the host", "Don't care");
			return;
		}

		Transform parent = playerUIPrefabParent.transform;

		GameObject objectToRemove = null;

		foreach (Player_UI playerUI in parent.GetComponentsInChildren<Player_UI>())
			if (playerUI.PlayerId.Equals(playerId)) objectToRemove = playerUI.gameObject;

		if (objectToRemove != null)
			Destroy(objectToRemove);
	}

	public void AddPlayerToLobbyUI(string playerId)
	{
		GameObject newPlayerUI = Instantiate(playerUIPrefab, playerUIPrefabParent.transform);

		newPlayerUI.GetComponent<Player_UI>().PlayerId = playerId;

		SetPlayerObject(newPlayerUI, playerId);
	}

	public void UpdatePlayerInLobbyUI(string playerId)
	{
		Transform parent = playerUIPrefabParent.transform;

		GameObject objectToUpdate = null;

		foreach (Player_UI playerUI in parent.GetComponentsInChildren<Player_UI>())
			if (playerUI.PlayerId.Equals(playerId)) objectToUpdate = playerUI.gameObject;

		SetPlayerObject(objectToUpdate, playerId);
	}

	private IEnumerator SetPlayerObject(GameObject playerObj, string playerId)
	{
		Player player = null;
		yield return playerManager.GetPlayer(playerId, (dbplayer) =>
		{
			player = dbplayer;
		});

		string currentHost = null;
		yield return lobbyManager.GetHost(CurrentLobbyId, (dbhost) =>
		{
			currentHost = dbhost;
		});

		bool playerIsHost = player.Id.Equals(currentHost);

		PlayerColor playerColor = PlayerColor.Red;
		yield return lobbyManager.GetPlayerColor(CurrentLobbyId, playerId, (dbplayercolor) =>
		{
			playerColor = dbplayercolor;
		});


		Color color = lobbyManager.GetColor(playerColor);

		playerObj.transform.Find("NameAndIcon").Find("PlayerNameHolder").Find("PlayerName").GetComponent<TMP_Text>().text = player.Name;

		playerObj.transform.Find("NameAndIcon").Find("PlayerNameHolder").Find("PlayerName").GetComponent<TMP_Text>().color = color;

		playerObj.transform.Find("NameAndIcon").Find("HostIcon").gameObject.SetActive(playerIsHost);

		if (player.Id.Equals(playerManager.Client.Id))
			playerObj.transform.Find("KickAndPromote").gameObject.SetActive(false);
		else
			playerObj.transform.Find("KickAndPromote").gameObject.SetActive(ClientIsHost);

		yield return new WaitForSeconds(0.02f);

		LayoutRebuilder.ForceRebuildLayoutImmediate(playerObj.transform.Find("NameAndIcon").GetComponent<RectTransform>());
	}

	public void PromotePlayer(string playerId)
	{
		lobbyManager.SetHost(currentLobbyId, playerId);
	}

	public IEnumerator ChangePlayerColor()
	{
		yield return lobbyManager.GetNextPlayerColor(playerManager.Client.Id, CurrentLobbyId, (color) => { });
	}
}
