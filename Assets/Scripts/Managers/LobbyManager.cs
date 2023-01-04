using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	public IEnumerator RetrieveData()
	{
		List<Lobby> newLobbies = null;
		yield return databaseManager.GetLobbies((lobbyList) =>
		{
			newLobbies = lobbyList;
		});

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

			foreach (Lobby lobby in newLobbies)
			{
				changedLobbies.Add(lobby.Id, LobbyChangeState.New);
			}
		}

		Lobbies = newLobbies;

		if (OnLobbiesChanged != null && changedLobbies.Count > 0)
			OnLobbiesChanged.Invoke(this, new LobbiesChangedEventArgs() { ChangedLobbies = changedLobbies });

	}

	public IEnumerator RemoveEmptyLobbies()
	{
		yield return databaseManager.RemoveEmptyLobbies();
	}

	public IEnumerator CreateLobby(Action<Lobby> callback)
	{
		Lobby lobby = null;
		yield return databaseManager.CreateLobby(playerManager.Client,
			$"{playerManager.Client.Name}'s lobby",
			$"Join {playerManager.Client.Name}'s lobby to feel accomplished",
			mapManager.DefaultMap,
			(dblobby) =>
			{
				lobby = dblobby;
			});
		callback.Invoke(lobby);
	}

	public IEnumerator CheckIfJoinable(string playerId, string lobbyId, Action<LobbyStatusMessage> callback)
	{
		Lobby lobby = null;
		yield return databaseManager.GetLobby(lobbyId, (dblobby) =>
		{
			lobby = dblobby;
		});

		if (lobby is null)
		{
			callback.Invoke(LobbyStatusMessage.Deleted);
			yield break;
		}

		int maxPlayers = mapManager.GetMap(lobby.MapId).maxPlayers;
		
		//TODO: Check if lobbystatus is open and not closed or started

		if (lobby.Players.Find((x) => x == playerId) is not null)
		{
			callback.Invoke(LobbyStatusMessage.Open);
			yield break;
		}
		else if (lobby.Players.Count < maxPlayers)
		{
			callback.Invoke(LobbyStatusMessage.Open);
			yield break;
		}
		callback.Invoke(LobbyStatusMessage.Full);
	}

	public IEnumerator GetLobby(string id, Action<Lobby> callback)
	{
		Lobby lobby = null;
		yield return databaseManager.GetLobby(id, (dbLobby) => lobby = dbLobby);
		callback.Invoke(lobby);
	}

	public IEnumerator GetLobbyPlayers(string lobbyId, Action<List<Player>> callback)
	{
		yield return databaseManager.GetLobbyPlayers(lobbyId, (players) =>
		{
			callback.Invoke(players);
		});
	}

	public Dictionary<string, LobbyChangeState> GetLobbyPlayersChanges(List<Player> oldPlayerList, List<Player> newPlayerList, Dictionary<string, PlayerColor> oldColors, Dictionary<string, PlayerColor> newColors)
	{
		Dictionary<string, LobbyChangeState> result = new();
		//Check for removals
		foreach (Player oldPlayer in oldPlayerList)
		{
			Player foundOldPlayer = newPlayerList.Find((newplayer) => newplayer.Id.Equals(oldPlayer.Id));
			if (foundOldPlayer is null)
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
					else if (oldColors.ContainsKey(newPlayer.Id) && !oldColors[newPlayer.Id].Equals(newColors[newPlayer.Id]))
					{
						//Player color changed
						result.Add(newPlayer.Id, LobbyChangeState.Changed);
					}
				}
			}
		}

		return result;
	}

	public IEnumerator JoinLobby(string lobbyId)
	{
		yield return databaseManager.AddPlayerToLobby(lobbyId, playerManager.Client.Id);
	}

	public IEnumerator KickPlayer(string lobbyId, string playerId)
	{
		yield return databaseManager.RemovePlayerFromLobby(lobbyId, playerId);
	}

	public IEnumerator UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp)
	{
		yield return databaseManager.UpdateLobbyLastSeen(playerId, lobbyId, timestamp);
		yield return databaseManager.RemoveInactivePlayersFromLobby(lobbyId, timeoutSeconds);
	}

	public IEnumerator FindAndRemoveInactivePlayers(string lobbyId)
	{
		yield return databaseManager.RemoveInactivePlayersFromLobby(lobbyId, timeoutSeconds);
	}

	public IEnumerator FindAndRemoveInactivePlayers()
	{
		List<Lobby> lobbies = null;
		yield return databaseManager.GetLobbies((lobbyList) =>
		{
			lobbies = lobbyList;
		});

		foreach (Lobby lobby in lobbies)
		{
			yield return FindAndRemoveInactivePlayers(lobby.Id);
		}
	}

	public IEnumerator RefreshLobbies()
	{
		yield return RetrieveData();
	}

	public IEnumerator GetHost(string lobbyId, Action<string> callback)
	{
		string hostId = null;
		yield return databaseManager.GetLobbyHost(lobbyId, (hostid) =>
		{
			hostId = hostid;
		});

		callback.Invoke(hostId);
	}

	public IEnumerator ClientIsHost(string lobbyId, Action<bool> callback)
	{
		string hostId = null;

		yield return GetHost(lobbyId, (hostid) => hostId = hostid);

		callback.Invoke(hostId.Equals(playerManager.Client.Id));
	}

	public IEnumerator SetHost(string lobbyId, string playerId)
	{
		yield return databaseManager.SetHost(lobbyId, playerId);
	}

	public IEnumerator GetPlayerColors(string lobbyId, Action<Dictionary<string, PlayerColor>> callback)
	{
		yield return databaseManager.GetLobbyColors(lobbyId, (playerColors) =>
		{
			callback.Invoke(playerColors);
		});
	}

	public IEnumerator GetPlayerColor(string lobbyId, string playerId, Action<PlayerColor> callback)
	{
		yield return databaseManager.GetPlayerColor(lobbyId, playerId, (playerColor) =>
		{
			callback.Invoke(playerColor);
		});
	}

	public Color GetColor(PlayerColor playerColor)
	{
		switch (playerColor)
		{
			case PlayerColor.Red:
				return Color.red;
			case PlayerColor.Blue:
				return Color.blue;
			case PlayerColor.Green:
				return Color.green;
			case PlayerColor.Orange:
				return new Color(226, 135, 67);
			case PlayerColor.Yellow:
				return Color.yellow;
			case PlayerColor.Unset:
				return Color.red;
			case PlayerColor.Purple:
				return new Color(153, 0, 204);
			default:
				return Color.red;
		}
	}

	/// <summary>
	/// Gets the player color and updates this in the database
	/// </summary>
	/// <param name="playerId"></param>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public IEnumerator GetNextPlayerColor(string playerId, string lobbyId, Action<Color> callback)
	{
		List<PlayerColor> availablePlayerColors = null;
		yield return GetAvailableColors(lobbyId, (playercolors) =>
		{
			availablePlayerColors = playercolors;
		});

		PlayerColor newColor = availablePlayerColors.FirstOrDefault();

		yield return databaseManager.UpdatePlayerColor(lobbyId, playerId, newColor);

		callback.Invoke(GetColor(newColor));
	}

	private IEnumerator GetAvailableColors(string lobbyId, Action<List<PlayerColor>> callback)
	{
		Dictionary<string, PlayerColor> playerColors = null;
		yield return GetPlayerColors(lobbyId, (dbplayercolors) =>
		{
			playerColors = dbplayercolors;
		});

		PlayerColor[] allColorsArr = (PlayerColor[])Enum.GetValues(typeof(PlayerColor));

		List<PlayerColor> allColorsList = allColorsArr.ToList();

		foreach(var playerColor in playerColors)
		{
			allColorsList.Remove(playerColor.Value);
		}

		if (allColorsList.Contains(PlayerColor.Unset))
			allColorsList.Remove(PlayerColor.Unset);

		callback.Invoke(allColorsList);
	}
}
