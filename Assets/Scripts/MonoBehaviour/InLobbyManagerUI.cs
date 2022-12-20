using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InLobbyManagerUI : MonoBehaviour
{
	public TMP_Text titleText;

	public GameObject playerUIPrefabParent;

	public GameObject playerUIPrefab;

	public float inLobbyRefreshTime = 3;

	public string CurrentLobbyId { get; set; }

	private LobbyManager lobbyManager;

	private float inLobbyRefreshTimeElapsed = 0;

	private void Start()
	{
		lobbyManager = GameManager.GetGameManager().dataManager.lobbyManager;

		lobbyManager.OnLobbiesChanged += LobbyManager_LobbiesChanged;
	}

	private void Update()
	{
		DoTimedUpdate();
	}

	private void DoTimedUpdate()
	{
		if (inLobbyRefreshTimeElapsed >= inLobbyRefreshTime)
		{
			if (playerUIPrefabParent.activeSelf)
			{
				lobbyManager.RefreshLobbies();
			}
			inLobbyRefreshTimeElapsed = 0;
		}
		else
		{
			inLobbyRefreshTimeElapsed += Time.deltaTime;
		}
	}

	private void LobbyManager_LobbiesChanged(object sender, LobbiesChangedEventArgs e)
	{
		
	}
}
