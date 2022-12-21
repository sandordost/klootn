using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LobbyManager : MonoBehaviour, IDataRecievable
{
	public int maxPlayers = 8;
	public int timeoutSeconds = 30;
	public event EventHandler<LobbiesChangedEventArgs> OnLobbiesChanged;

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
		List<Lobby> newLobbies = await databaseManager.GetLobbies();

		Dictionary<Lobby, LobbyChangeState> changedLobbies = new();

		if (Lobbies != null)
		{
			//Check if any old lobby is not in newlobbies (removed)
			foreach (Lobby lobby in Lobbies)
				if (newLobbies.Find((x) => x.Id.Equals(lobby.Id)) == null)
					changedLobbies.Add(lobby, LobbyChangeState.Deleted);

			foreach (Lobby newLobby in newLobbies)
			{
				//Check if newlobby already exists
				Lobby existingLobby = Lobbies.Find((x) => x.Id.Equals(newLobby.Id));
				
				if (existingLobby != null)
				{
					//newlobby exists
					if (!existingLobby.CompareLobby(newLobby))
					{
						//newlobby has changed
						changedLobbies.Add(newLobby, LobbyChangeState.Changed);
					}
				}
				else
				{
					//newlobby doesnt exist and is new
					changedLobbies.Add(newLobby, LobbyChangeState.New);
				}
			}
		}
		else
		{
			changedLobbies = new Dictionary<Lobby, LobbyChangeState>();
			foreach(Lobby lobby in newLobbies)
			{
				changedLobbies.Add(lobby, LobbyChangeState.New);
			}
		}

		Lobbies = newLobbies;

		if (OnLobbiesChanged != null && changedLobbies.Count > 0)
			OnLobbiesChanged.Invoke(this, new LobbiesChangedEventArgs() { ChangedLobbies = changedLobbies} );

	}

	public async Task<Lobby> CreateLobby()
	{
		return await databaseManager.CreateLobby(playerManager.Client,
			$"{playerManager.Client.Name}'s lobby",
			$"Join {playerManager.Client.Name}'s lobby to feel accomplished");
	}

	public async Task<Lobby> GetLobby(string id)
	{
		Lobby lobby = await databaseManager.GetLobby(id);

		return lobby;
	}

	public async void JoinLobby(string lobbyId)
	{
		await databaseManager.AddPlayerToLobby(lobbyId, playerManager.Client.Id);
	}

	public async void UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp)
	{
		await databaseManager.UpdateLobbyLastSeen(playerId, lobbyId, timestamp);
		await databaseManager.RemoveInactivePlayersFromLobby(lobbyId, timeoutSeconds);
	}

	public async void FindAndRemoveInactivePlayers(string lobbyId)
	{
		await databaseManager.RemoveInactivePlayersFromLobby(lobbyId, timeoutSeconds);
	}

	public async void RefreshLobbies()
	{
		await RetrieveData();
	}
}
