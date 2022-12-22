using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using TMPro;
using UnityEngine;

public class InLobbyManagerUI : MonoBehaviour
{
	public TMP_Text titleText;

	public GameObject inLobbyPage;

	public GameObject playerUIPrefabParent;

	public GameObject playerUIPrefab;

	private LobbyManager lobbyManager;

	private PlayerManager playerManager;

	public float inLobbyRefreshTime = 3;
	private float inLobbyRefreshTimeElapsed = 0;

	private string currentLobbyId;

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

	private void ClearPlayerListUI()
	{
		foreach(Transform t in playerUIPrefabParent.transform)
		{
			Destroy(t.gameObject);
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

	private void Start()
	{
		GameManager gameManager = GameManager.GetGameManager();

		lobbyManager = gameManager.dataManager.lobbyManager;

		playerManager = gameManager.dataManager.playerManager;
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
		lobbyManager.UpdateLobbyLastSeen(playerManager.Client.Id, currentLobbyId, Timestamp.GetCurrentTimestamp());
		UpdatePlayerListUI();
		inLobbyRefreshTimeElapsed = 0;
	}

	private void LobbyManager_LobbiesChanged(object sender, LobbiesChangedEventArgs e)
	{
		UpdateInLobbyUI();
	}

	private async void UpdateInLobbyUI()
	{
		//Change inlobbyUI
		titleText.text = "";

		Lobby lobby = await lobbyManager.GetLobby(currentLobbyId);

		titleText.text = lobby.Name;
	}

	private async void UpdatePlayerListUI()
	{
		//Change Player area
		List<Player> lobbyPlayers = await lobbyManager.GetLobbyPlayers(CurrentLobbyId);

		Dictionary<string, LobbyChangeState> lobbyPlayerChanges = lobbyManager.GetLobbyPlayersChanges(CurrentPlayers, lobbyPlayers);

		foreach (var playerChange in lobbyPlayerChanges)
		{
			switch (playerChange.Value)
			{
				case LobbyChangeState.New:
					AddPlayerToLobby(playerChange.Key);
					break;
				case LobbyChangeState.Changed:
					UpdatePlayerInLobby(playerChange.Key);
					break;
				case LobbyChangeState.Deleted:
					RemovePlayerFromLobby(playerChange.Key);
					break;
			}
		}

		CurrentPlayers = lobbyPlayers;
	}

	private void RemovePlayerFromLobby(string playerId)
	{
		Transform parent = playerUIPrefabParent.transform;

		GameObject objectToRemove = null;

		foreach (Player_UI playerUI in parent.GetComponentsInChildren<Player_UI>())
			if (playerUI.PlayerId.Equals(playerId)) objectToRemove = playerUI.gameObject;

		if (objectToRemove is not null)
			Destroy(objectToRemove);
	}

	public async void AddPlayerToLobby(string playerId)
	{
		Player player = await playerManager.GetPlayer(playerId);

		GameObject newPlayerUI = Instantiate(playerUIPrefab, playerUIPrefabParent.transform);

		newPlayerUI.transform.Find("NameAndIcon").Find("PlayerName").GetComponent<TMP_Text>().text = player.Name;

		newPlayerUI.GetComponent<Player_UI>().PlayerId = playerId;
	}

	public async void UpdatePlayerInLobby(string playerId)
	{
		Player player = await playerManager.GetPlayer(playerId);

		Transform parent = playerUIPrefabParent.transform;

		GameObject objectToUpdate = null;

		foreach (Player_UI playerUI in parent.GetComponentsInChildren<Player_UI>())
			if (playerUI.PlayerId.Equals(playerId)) objectToUpdate = playerUI.gameObject;

		objectToUpdate.transform.Find("NameAndIcon").Find("PlayerName").GetComponent<TMP_Text>().text = player.Name;


	}
}
