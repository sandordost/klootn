using System;
using System.Collections;
using System.Collections.Generic;
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
			UpdateInLobbyUI();
		} 
	}

	private void Start()
	{
		lobbyManager = GameManager.GetGameManager().dataManager.lobbyManager;

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

	private async void UpdateInLobbyUI()
	{
		ClearInLobbyUI();
		Lobby lobby = await lobbyManager.GetLobby(CurrentLobbyId);
		UpdateInLobbyUI(lobby);
	}

	private void ClearInLobbyUI()
	{
		titleText.text = "";
	}

	private void UpdateInLobbyUI(Lobby lobby)
	{
		foreach(Transform t in playerUIPrefabParent.transform)
		{
			Destroy(t.gameObject);
		}

		titleText.text = lobby.Name;

		foreach(var player in lobby.Players)
		{
			GameObject newPlayerUI = Instantiate(playerUIPrefab, playerUIPrefabParent.transform);

			newPlayerUI.transform.Find("NameAndIcon").Find("PlayerName").GetComponent<TMP_Text>().text = player.Value.Name;
		}
	}
}
