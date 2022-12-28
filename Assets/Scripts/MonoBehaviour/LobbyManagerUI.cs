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

		lobbyRefreshTimeElapsed = lobbyRefreshTime;
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
