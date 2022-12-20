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

		lobbyManager.OnLobbiesChanged += LobbiesChanged;
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
		lobbyManager.RefreshLobbies();

		pageSwitcher.SwitchPage(inLobbyPage);
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

	private void RemoveLobbyFromUI(Lobby lobby)
	{
		foreach (Transform transform in lobbyPrefabParent.transform)
		{
			Destroy(transform.gameObject);
		}
	}

	private void UpdateLobbyUI(Lobby lobby)
	{
		GameObject existingLobby = FindLobby(lobby.Id);

		existingLobby.transform.Find("LobbyTitle").GetComponent<TMP_Text>().text = lobby.Name;
		existingLobby.transform.Find("LobbyDescription").GetComponent<TMP_Text>().text = lobby.Description;
		existingLobby.transform.Find("LobbyPlayers").GetComponent<TMP_Text>().text = $"{lobby.Players.Count}/{lobbyManager.maxPlayers}";
	}

	private GameObject FindLobby(string id)
	{
		foreach(LobbyClick click in lobbyPrefabParent.GetComponentsInChildren<LobbyClick>())
		{
			if (click.LobbyId.Equals(id)) return click.gameObject;
		}
		return null;
	}

	private void AddLobbyToUI(Lobby lobby)
	{
		GameObject newLobby = Instantiate(lobbyPrefab, lobbyPrefabParent.transform);

		newLobby.GetComponent<LobbyClick>().LobbyId = lobby.Id;

		newLobby.transform.Find("LobbyTitle").GetComponent<TMP_Text>().text = lobby.Name;
		newLobby.transform.Find("LobbyDescription").GetComponent<TMP_Text>().text = lobby.Description;
		newLobby.transform.Find("LobbyPlayers").GetComponent<TMP_Text>().text = $"{lobby.Players.Count}/{lobbyManager.maxPlayers}";
	}

}
