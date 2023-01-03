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
				currentPlayers = new();
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
			if (currentPlayerColors is null)
			{
				currentPlayerColors = new();
			}
			return currentPlayerColors;
		}
		set
		{
			currentPlayerColors = value;
		}
	}

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
		if (CurrentLobbyId is not null && e.ChangedLobbies.ContainsKey(CurrentLobbyId))
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
				RefreshAndUpdate();
			}
			else
			{
				inLobbyRefreshTimeElapsed += Time.deltaTime;
			}
		}
	}

	private void RefreshAndUpdate()
	{
		Debug.Log("Updating Lobbies from inLobbyUI");
		lobbyManager.RefreshLobbies();
		lobbyManager.UpdateLobbyLastSeen(playerManager.Client.Id, CurrentLobbyId, Timestamp.GetCurrentTimestamp());
		UpdatePlayerListUI();
		inLobbyRefreshTimeElapsed = 0;
	}

	private async void UpdateInLobbyUI()
	{
		alertManager.ShowLoadingAlert("Loading lobby ...");

		Lobby lobby = await lobbyManager.GetLobby(CurrentLobbyId);

		if (lobby is null)
		{
			alertManager.CloseLoadingAlert();
			uiPageSwitcher.SwitchPage("LobbyPage");
			return;
		}

		ClientIsHost = await lobbyManager.ClientIsHost(lobby.Id);

		KlootnMap map = await mapManager.GetCurrentKlootnMap(CurrentLobbyId);

		mapGameObj.transform.Find("Image").GetComponent<Image>().sprite = map.image;

		mapGameObj.transform.Find("MapInfo").Find("MapName").GetComponent<TMP_Text>().text = map.title;

		mapGameObj.transform.Find("MapInfo").Find("MapDescription").GetComponent<TMP_Text>().text = map.description;

		titleText.text = lobby.Name;

		UpdateHostControls(ClientIsHost);

		alertManager.CloseLoadingAlert();
	}

	private async void UpdateHostControls(bool clientIsHost)
	{
		Transform parent = playerUIPrefabParent.transform;

		mapChangeButton.SetActive(clientIsHost);

		string currentHost = await lobbyManager.GetHost(CurrentLobbyId);

		foreach (Player_UI playerUI in parent.GetComponentsInChildren<Player_UI>())
		{
			GameObject playerCard = playerUI.gameObject;

			playerCard.transform.Find("NameAndIcon").Find("HostIcon").gameObject.SetActive(currentHost.Equals(playerUI.PlayerId));

			playerCard.transform.Find("KickAndPromote").gameObject.SetActive(clientIsHost && playerUI.PlayerId != playerManager.Client.Id);
		}
	}

	private async void UpdatePlayerListUI()
	{
		//Change Player area
		List<Player> lobbyPlayers = await lobbyManager.GetLobbyPlayers(CurrentLobbyId);

		Dictionary<string, PlayerColor> newColors = await lobbyManager.GetPlayerColors(CurrentLobbyId);

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

		if (objectToRemove is not null)
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

	private async void SetPlayerObject(GameObject playerObj, string playerId)
	{
		Player player = await playerManager.GetPlayer(playerId);

		string currentHost = await lobbyManager.GetHost(CurrentLobbyId);

		bool playerIsHost = player.Id.Equals(currentHost);

		PlayerColor playerColor = await lobbyManager.GetPlayerColor(CurrentLobbyId, playerId);

		Color color = lobbyManager.GetColor(playerColor);

		playerObj.transform.Find("NameAndIcon").Find("PlayerNameHolder").Find("PlayerName").GetComponent<TMP_Text>().text = player.Name;

		playerObj.transform.Find("NameAndIcon").Find("PlayerNameHolder").Find("PlayerName").GetComponent<TMP_Text>().color = color;

		playerObj.transform.Find("NameAndIcon").Find("HostIcon").gameObject.SetActive(playerIsHost);

		if (player.Id.Equals(playerManager.Client.Id))
			playerObj.transform.Find("KickAndPromote").gameObject.SetActive(false);
		else
			playerObj.transform.Find("KickAndPromote").gameObject.SetActive(ClientIsHost);

		await Task.Delay(20);

		LayoutRebuilder.ForceRebuildLayoutImmediate(playerObj.transform.Find("NameAndIcon").GetComponent<RectTransform>());
	}

	public void PromotePlayer(string playerId)
	{
		lobbyManager.SetHost(currentLobbyId, playerId);
	}

	public async void ChangePlayerColor()
	{
		await lobbyManager.GetNextPlayerColor(playerManager.Client.Id, CurrentLobbyId);
	}
}
