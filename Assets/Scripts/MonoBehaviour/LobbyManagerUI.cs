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

	private LobbyManager lobbyManager;
	private InLobbyManagerUI inLobbyManagerUI;

	public float lobbyRefreshTime = 5;
	private float lobbyRefreshTimeElapsed = 0;

	void Start()
	{
		UIManager uiManager = UIManager.GetInstance();
		GameManager gameManager = GameManager.GetGameManager();
		lobbyManager = gameManager.dataManager.lobbyManager;
		inLobbyManagerUI = uiManager.inLobbyManagerUI;

		lobbyManager.OnLobbiesChanged += LobbiesChanged;
	}

	private void FixedUpdate()
	{
		if (lobbyPage.activeInHierarchy)
		{
			if (lobbyRefreshTimeElapsed > lobbyRefreshTime)
			{
				Debug.Log("Updating Lobbies from LobbyUI");
				lobbyManager.RefreshLobbies();
				lobbyRefreshTimeElapsed = 0;
			}
			else
			{
				lobbyRefreshTimeElapsed += Time.deltaTime;
			}
		}
	}

	public async void CreateNewLobby()
	{
		Lobby lobby = await lobbyManager.CreateLobby();
		OpenLobbyUI(lobby.Id);
	}

	public void OpenLobbyUI(string lobbyId)
	{
		lobbyManager.RefreshLobbies();

		inLobbyManagerUI.CurrentLobbyId = lobbyId;

		lobbyManager.JoinLobby(lobbyId);

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

	private async void RemoveLobbyFromUI(string lobbyId)
	{
		Lobby lobby = await lobbyManager.GetLobby(lobbyId);

		foreach (Transform transform in lobbyPrefabParent.transform)
		{
			if (transform.GetComponent<LobbyClick>().LobbyId.Equals(lobby.Id))
			{
				Destroy(transform.gameObject);
				return;
			}
		}
	}

	private async void UpdateLobbyUI(string lobbyId)
	{
		Lobby lobby = await lobbyManager.GetLobby(lobbyId);

		GameObject existingLobby = FindLobbyCardUI(lobby.Id);

		existingLobby.transform.Find("LobbyTitle").GetComponent<TMP_Text>().text = lobby.Name;
		existingLobby.transform.Find("LobbyDescription").GetComponent<TMP_Text>().text = lobby.Description;
		existingLobby.transform.Find("LobbyPlayers").GetComponent<TMP_Text>().text = $"{lobby.Players.Count}/{lobbyManager.maxPlayers}";
	}

	private async void AddLobbyToUI(string lobbyId)
	{
		Lobby lobby = await lobbyManager.GetLobby(lobbyId);

		GameObject newLobby = Instantiate(lobbyPrefab, lobbyPrefabParent.transform);

		newLobby.GetComponent<LobbyClick>().LobbyId = lobby.Id;

		newLobby.transform.Find("LobbyTitle").GetComponent<TMP_Text>().text = lobby.Name;
		newLobby.transform.Find("LobbyDescription").GetComponent<TMP_Text>().text = lobby.Description;
		newLobby.transform.Find("LobbyPlayers").GetComponent<TMP_Text>().text = $"{lobby.Players.Count}/{lobbyManager.maxPlayers}";
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
