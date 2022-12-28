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
	private MapManager mapManager;
	private List<Lobby> Lobbies { get; set; }


	private void Start()
	{
		GameManager gameManager = GameManager.GetInstance();

		mapManager = gameManager.dataManager.mapManager;
		databaseManager = gameManager.dataManager.databaseManager;
		playerManager = gameManager.dataManager.playerManager;
	}

	public async Task RetrieveData()
	{
		List<Lobby> newLobbies = await databaseManager.GetLobbies();

		Dictionary<string, LobbyChangeState> changedLobbies = new();

		if (Lobbies != null)
		{
			//Check if any old lobby is not in newlobbies (removed)
			foreach (Lobby lobby in Lobbies)
				if (newLobbies.Find((x) => x.Id.Equals(lobby.Id)) == null)
					changedLobbies.Add(lobby.Id, LobbyChangeState.Deleted);

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
						changedLobbies.Add(newLobby.Id, LobbyChangeState.Changed);
					}
				}
				else
				{
					//newlobby doesnt exist and is new
					changedLobbies.Add(newLobby.Id, LobbyChangeState.New);
				}
			}
		}
		else
		{
			changedLobbies = new Dictionary<string, LobbyChangeState>();

			foreach(Lobby lobby in newLobbies)
			{
				changedLobbies.Add(lobby.Id, LobbyChangeState.New);
			}
		}

		Lobbies = newLobbies;

		if (OnLobbiesChanged != null && changedLobbies.Count > 0)
			OnLobbiesChanged.Invoke(this, new LobbiesChangedEventArgs() { ChangedLobbies = changedLobbies} );

	}

	public async void RemoveEmptyLobbies()
	{
		await databaseManager.RemoveEmptyLobbies();
	}

	public async Task<Lobby> CreateLobby()
	{
		return await databaseManager.CreateLobby(playerManager.Client,
			$"{playerManager.Client.Name}'s lobby",
			$"Join {playerManager.Client.Name}'s lobby to feel accomplished",
			mapManager.DefaultMap);
	}

	public async Task<Lobby> GetLobby(string id)
	{
		Lobby lobby = await databaseManager.GetLobby(id);

		return lobby;
	}

	public async Task<List<Player>> GetLobbyPlayers(string lobbyId)
	{
		return await databaseManager.GetLobbyPlayers(lobbyId);
	}

	public Dictionary<string, LobbyChangeState> GetLobbyPlayersChanges(List<Player> oldPlayerList, List<Player> newPlayerList)
	{
		Dictionary<string, LobbyChangeState> result = new();
		//Check for removals
		foreach(Player oldPlayer in oldPlayerList)
		{
			Player foundOldPlayer = newPlayerList.Find((newplayer) => newplayer.Id.Equals(oldPlayer.Id));
			if(foundOldPlayer is null)
			{
				//Old player not found in the newplayer list
				result.Add(oldPlayer.Id, LobbyChangeState.Deleted);
			}
		}

		//Check for adds or updates
		if (newPlayerList is not null)
		{
			foreach (Player newPlayer in newPlayerList)
			{
				//Find new player in old player list
				Player foundNewPlayer = oldPlayerList.Find((oldplayer) => oldplayer.Id.Equals(newPlayer.Id));
				if (foundNewPlayer is null)
				{
					//New player is not found in the oldplayer list
					result.Add(newPlayer.Id, LobbyChangeState.New);
				}
				else
				{
					//Player exists - check for changes in player
					if (!foundNewPlayer.ComparePlayer(newPlayer))
					{
						//Player changed
						result.Add(newPlayer.Id, LobbyChangeState.Changed);
					}
				}
			}
		}

		return result;
	}

	public async void JoinLobby(string lobbyId)
	{
		await databaseManager.AddPlayerToLobby(lobbyId, playerManager.Client.Id);
	}

	public async void KickPlayer(string lobbyId, string playerId)
	{
		await databaseManager.RemovePlayerFromLobby(lobbyId, playerId);
	}

	public async void UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp)
	{
		await databaseManager.UpdateLobbyLastSeen(playerId, lobbyId, timestamp);
		await databaseManager.RemoveInactivePlayersFromLobby(lobbyId, timeoutSeconds);
	}

	public async Task FindAndRemoveInactivePlayers(string lobbyId)
	{
		await databaseManager.RemoveInactivePlayersFromLobby(lobbyId, timeoutSeconds);
	}

	public async Task FindAndRemoveInactivePlayers()
	{
		List<Lobby> lobbies = await databaseManager.GetLobbies();

		foreach (Lobby lobby in lobbies)
		{
			await FindAndRemoveInactivePlayers(lobby.Id);
		}
	}

	public async void RefreshLobbies()
	{
		await RetrieveData();
	}

	public async Task<string> GetHost(string lobbyId)
	{
		string hostId = await databaseManager.GetLobbyHost(lobbyId);

		return hostId;
	}

	public async Task<bool> ClientIsHost(string lobbyId)
	{
		string hostId = await GetHost(lobbyId);

		return hostId.Equals(playerManager.Client.Id);
	}

	public async void SetHost(string lobbyId, string playerId)
	{
		await databaseManager.SetHost(lobbyId, playerId);
	}
}
