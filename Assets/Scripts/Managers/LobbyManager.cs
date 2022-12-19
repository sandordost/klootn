using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LobbyManager : MonoBehaviour, IDataRecievable
{
	IDatabaseManager databaseManager;
	PlayerManager playerManager;

	public List<Lobby> lobbies { get; set; }
	public int maxPlayers = 8;
	public event EventHandler<List<Lobby>> OnLobbiesFetched;

	private void Start()
	{
		GameManager gameManager = GameManager.GetGameManager();

		databaseManager = gameManager.dataManager.databaseManager;
		playerManager = gameManager.dataManager.playerManager;
	}

	public async Task RetrieveData()
	{
		lobbies = await databaseManager.GetLobbies();
		if (OnLobbiesFetched != null && lobbies.Count > 0)
			OnLobbiesFetched.Invoke(this, lobbies);
	}

	public async Task<Lobby> CreateLobby()
	{
		return await databaseManager.CreateLobby(playerManager.Client, 
			$"{playerManager.Client.name}'s lobby", 
			$"Join {playerManager.Client.name}'s lobby to feel accomplished");
	}
}
