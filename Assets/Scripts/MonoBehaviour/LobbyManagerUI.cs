using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LobbyManagerUI : MonoBehaviour
{
	public GameObject inLobbyPage;
	public GameObject lobbyPage;
	public GameObject lobbyPrefabParent;
	public GameObject lobbyPrefab;
	public UIPageSwitcher pageSwitcher;

	private LobbyManager lobbyManager;

	public float lobbyRefreshTime = 5;
	private float lobbyRefreshTimeElapsed = 0;

	void Start()
	{
		GameManager gameManager = GameManager.GetGameManager();
		lobbyManager = gameManager.dataManager.lobbyManager;

		lobbyManager.OnLobbiesFetched += LobbiesFetched;
	}

	private void FixedUpdate()
	{
		DoTimedUpdate();
	}

	private void DoTimedUpdate()
	{
		if (lobbyRefreshTimeElapsed >= lobbyRefreshTime)
		{
			if (lobbyPage.activeSelf)
			{
				RefreshLobbies();
			}
			lobbyRefreshTimeElapsed = 0;
		}
		else
		{
			lobbyRefreshTimeElapsed += Time.deltaTime;
		}
	}

	public async void CreateNewLobby()
	{
		//TODO: Create new lobby and swap page to inLobbyPage
		Lobby lobby = await lobbyManager.CreateLobby();
		OpenLobbyUI(lobby.Id);
	}

	public void OpenLobbyUI(string lobbyId)
	{
		//TODO: Load lobby info into inLobbyPage before showing

		pageSwitcher.SwitchPage(inLobbyPage);
	}

	public void RefreshLobbies()
	{
		lobbyManager.RefreshLobbies();
	}

	private void ReloadLobbyUI(List<Lobby> lobbies)
	{
		RemoveAllUILobbiesAsync();

		AddLobbiesToUIAsync(lobbies);
	}

	private void LobbiesFetched(object sender, List<Lobby> lobbies)
	{
		ReloadLobbyUI(lobbies);
	}

	private void RemoveAllUILobbiesAsync()
	{
		foreach (Transform transform in lobbyPrefabParent.transform)
		{
			Destroy(transform.gameObject);
		}
	}

	private void AddLobbiesToUIAsync(List<Lobby> lobbies)
	{
		foreach (Lobby lobby in lobbies)
		{
			GameObject newLobby = Instantiate(lobbyPrefab, lobbyPrefabParent.transform);

			newLobby.GetComponent<LobbyClick>().LobbyId = lobby.Id;

			newLobby.transform.Find("LobbyTitle").GetComponent<TMP_Text>().text = lobby.Name;
			newLobby.transform.Find("LobbyDescription").GetComponent<TMP_Text>().text = lobby.Description;
			newLobby.transform.Find("LobbyPlayers").GetComponent<TMP_Text>().text = $"{lobby.Players.Count}/{lobbyManager.maxPlayers}";
		}
	}

}
