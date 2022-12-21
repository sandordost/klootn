using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class InLobbyManagerUI : MonoBehaviour
{
	public TMP_Text titleText;

	public GameObject inLobbyPage;

	public GameObject playerUIPrefabParent;

	public GameObject playerUIPrefab;

	public float inLobbyRefreshTime = 3;

	private LobbyManager lobbyManager;

	private PlayerManager playerManager;

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
			UpdateInLobbyUI(value);
		} 
	}

	private void Start()
	{
		GameManager gameManager = GameManager.GetGameManager();

		lobbyManager = gameManager.dataManager.lobbyManager;

		playerManager = gameManager.dataManager.playerManager;

		lobbyManager.OnLobbiesChanged += LobbyManager_LobbiesChanged;
	}

	private void Update()
	{
		TimedUpdate();
	}

	private void TimedUpdate()
	{
		if (inLobbyRefreshTimeElapsed >= inLobbyRefreshTime)
		{
			if (inLobbyPage.activeSelf)
			{
				DoTimedUpdate();
			}
			inLobbyRefreshTimeElapsed = 0;
		}
		else
		{
			inLobbyRefreshTimeElapsed += Time.deltaTime;
		}
	}

	private void DoTimedUpdate()
	{
		lobbyManager.RefreshLobbies();
		UpdateLobbyLastSeen();
	}

	private void UpdateLobbyLastSeen()
	{
		lobbyManager.UpdateLobbyLastSeen(playerManager.Client.Id, currentLobbyId, DateTime.Now);
	}

	private void LobbyManager_LobbiesChanged(object sender, LobbiesChangedEventArgs e)
	{
		foreach (Lobby lobby in e.ChangedLobbies.Keys)
		{
			if (lobby.Id == CurrentLobbyId)
			{
				UpdateInLobbyUI(lobby);
			}
		}
	}

	private async void UpdateInLobbyUI(string lobbyId)
	{
		Lobby lobby = await lobbyManager.GetLobby(lobbyId);

		UpdateInLobbyUI(lobby);
	}

	private async void UpdateInLobbyUI(Lobby lobby)
	{
		titleText.text = "";

		foreach (Transform t in playerUIPrefabParent.transform)
		{
			Destroy(t.gameObject);
		}

		titleText.text = lobby.Name;

		foreach(var playerId in lobby.Players)
		{
			GameObject newPlayerUI = Instantiate(playerUIPrefab, playerUIPrefabParent.transform);

			Player player = await playerManager.GetPlayer(playerId);

			newPlayerUI.transform.Find("NameAndIcon").Find("PlayerName").GetComponent<TMP_Text>().text = player.Name;
		}
	}
}
