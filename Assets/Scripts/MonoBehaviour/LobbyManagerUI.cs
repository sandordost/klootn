using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManagerUI : MonoBehaviour
{
	public GameObject lobbyPage;
	public GameObject lobbyPrefabParent;
	public GameObject lobbyPrefab;
	public UIPageSwitcher pageSwitcher;

	private LobbyManager lobbyManager;

	void Start()
	{
		GameManager gameManager = GameManager.GetGameManager();
		lobbyManager = gameManager.dataManager.lobbyManager;

		lobbyManager.OnLobbiesFetched += LobbiesFetched;
	}


	public async void CreateNewLobby()
	{
		//TODO: Create new lobby and swap page to inLobbyPage
		Lobby lobby = await lobbyManager.CreateLobby();
		OpenLobbyUI(lobby);
	}

	private void OpenLobbyUI(Lobby lobby)
	{
		//TODO: Load lobby info into inLobbyPage before showing

		pageSwitcher.SwitchPage(lobbyPage);
	}

	public void RefreshLobbyList()
	{
		//TODO: LobbyManager should retrieve data
	}

	private void LobbiesFetched(object sender, List<Lobby> e)
	{
		foreach (Lobby lobby in e)
		{
			GameObject newLobby = Instantiate(lobbyPrefab);
			newLobby.transform.SetParent(lobbyPrefab.transform);

			newLobby.transform.Find("LobbyTitle").GetComponent<TMP_Text>().text = lobby.Name;

			newLobby.transform.Find("LobbyDescription").GetComponent<TMP_Text>().text = lobby.Description;

			newLobby.transform.Find("LobbyPlayers").GetComponent<TMP_Text>().text = $"{lobby.Players.Count}/{lobbyManager.maxPlayers}";
		}
	}

}
