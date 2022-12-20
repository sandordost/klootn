using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LobbyManager : MonoBehaviour, IDataRecievable
{
	public int maxPlayers = 8;
	public event EventHandler<List<Lobby>> OnLobbiesFetched;

	private IDatabaseManager databaseManager;
	private PlayerManager playerManager;
	private List<Lobby> Lobbies { get; set; }
	

	private void Start()
	{
		GameManager gameManager = GameManager.GetGameManager();

		databaseManager = gameManager.dataManager.databaseManager;
		playerManager = gameManager.dataManager.playerManager;
	}

	public async Task RetrieveData()
	{
		List<Lobby> lobbies = await databaseManager.GetLobbies();

		Lobbies = lobbies;

		if (OnLobbiesFetched != null && Lobbies.Count > 0)
			OnLobbiesFetched.Invoke(this, Lobbies);

	}

	public async Task<Lobby> CreateLobby()
	{
		return await databaseManager.CreateLobby(playerManager.Client, 
			$"{playerManager.Client.Name}'s lobby", 
			$"Join {playerManager.Client.Name}'s lobby to feel accomplished");
	}

	public async void RefreshLobbies()
	{
		await RetrieveData();
	}
}
