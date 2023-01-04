//using Firebase.Firestore;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using UnityEngine;

//public class FirestoreManager : IDatabaseManager
//{
//	FirebaseFirestore firestore;

//	public FirestoreManager()
//	{
//		firestore = FirebaseFirestore.DefaultInstance;
//	}

//	public async Task<Motd> GetLatestMotd()
//	{
//		CollectionReference collectionReference = firestore.Collection("Motds");

//		var task = await collectionReference.OrderByDescending("Uploaddate").GetSnapshotAsync();

//		QuerySnapshot result = task;

//		Motd motd = null;

//		if (result.Count > 0)
//			motd = result[0].ConvertTo<Motd>();

//		return motd;
//	}

//	public async Task<List<Lobby>> GetLobbies()
//	{
//		CollectionReference colRef = firestore.Collection("Lobbies");

//		QuerySnapshot snapshot = await colRef.GetSnapshotAsync();

//		List<Lobby> result = new();

//		foreach (var item in snapshot.Documents)
//		{
//			result.Add(item.ConvertTo<Lobby>());
//		}

//		return result;
//	}

//	public async Task<Lobby> CreateLobby(Player host, string name, string description, string mapId)
//	{
//		Lobby lobby = new()
//		{
//			HostId = host.Id,
//			MapId = mapId,
//			Name = name,
//			Description = description,
//			Players = new(),
//			LobbyPlayersData = new(),
//		};

//		lobby.Players.Add(host.Id);

//		CollectionReference colRef = firestore.Collection("Lobbies");

//		DocumentReference newDocument = await colRef.AddAsync(lobby);

//		lobby.Id = newDocument.Id;

//		return lobby;
//	}

//	public async Task<Player> RegisterPlayer(Player newPlayer)
//	{
//		CollectionReference colRef = firestore.Collection("Players");

//		DocumentReference addedPlayer = await colRef.AddAsync(newPlayer);

//		DocumentReference docRef = addedPlayer;

//		Player result = await GetPlayerById(docRef.Id);

//		return result;
//	}

//	public async Task<Player> Login(Player newPlayer)
//	{
//		CollectionReference playersRef = firestore.Collection("Players");

//		Query query = playersRef.WhereEqualTo("Name", newPlayer.Name);
//		query = query.WhereEqualTo("Password", newPlayer.Password);

//		var task = await query.GetSnapshotAsync();

//		var result = task;

//		Player playerResult = null;

//		if (result.Count > 0)
//			playerResult = result[0].ConvertTo<Player>();

//		return playerResult;
//	}

//	public async Task<bool> PlayerExists(Player newPlayer)
//	{
//		Player player = await GetPlayerByName(newPlayer.Name);

//		return player != null;
//	}

//	public async Task<List<Player>> GetAllPlayers()
//	{
//		CollectionReference playersRef = firestore.Collection("Players");

//		QuerySnapshot snapshot = await playersRef.GetSnapshotAsync();

//		List<Player> result = new();

//		foreach (var item in snapshot.Documents)
//		{
//			result.Add(item.ConvertTo<Player>());
//		}

//		return result;
//	}

//	public async Task<List<Player>> GetAllPlayers(string[] playerIds)
//	{
//		if (playerIds is null || playerIds.Length.Equals(0))
//			return new();

//		CollectionReference playersRef = firestore.Collection("Players");

//		Query query = playersRef.WhereIn(FieldPath.DocumentId, playerIds);

//		QuerySnapshot snapshot = await query.GetSnapshotAsync();

//		List<Player> result = new();

//		foreach (var item in snapshot.Documents)
//		{
//			result.Add(item.ConvertTo<Player>());
//		}

//		return result;
//	}

//	public async Task<Player> GetPlayerById(string id)
//	{
//		CollectionReference playersRef = firestore.Collection("Players");

//		Query query = playersRef.WhereEqualTo(FieldPath.DocumentId, id);

//		var task = await query.GetSnapshotAsync();

//		QuerySnapshot result = task;

//		Player player = null;

//		if (result.Count > 0)
//			player = result[0].ConvertTo<Player>();

//		return player;
//	}

//	public async Task<Player> GetPlayerByName(string name)
//	{
//		CollectionReference playersRef = firestore.Collection("Players");

//		Query query = playersRef.WhereEqualTo("Name", name);

//		var task = await query.GetSnapshotAsync();

//		QuerySnapshot result = task;

//		Player player = null;

//		if (result.Count > 0)
//			player = result[0].ConvertTo<Player>();

//		return player;
//	}

//	public async Task<Lobby> GetLobby(string lobbyId)
//	{
//		CollectionReference lobbiesRef = firestore.Collection("Lobbies");

//		Query query = lobbiesRef.WhereEqualTo(FieldPath.DocumentId, lobbyId);

//		var task = await query.GetSnapshotAsync();

//		QuerySnapshot result = task;

//		Lobby lobby = null;

//		if (result.Count > 0)
//			lobby = result[0].ConvertTo<Lobby>();

//		return lobby;
//	}

//	public async Task AddPlayerToLobby(string lobbyId, string playerId)
//	{
//		Task<Player> getPlayerTask = GetPlayerById(playerId);
//		Task<Lobby> getLobbyTask = GetLobby(lobbyId);

//		await Task.WhenAll(new Task[] { getPlayerTask, getLobbyTask });

//		Player player = getPlayerTask.Result;

//		Lobby lobby = getLobbyTask.Result;

//		if (!lobby.Players.Contains(player.Id))
//		{
//			lobby.Players.Add(player.Id);

//			await UpdateLobby(lobbyId, lobby);
//		}
//	}

//	private async Task UpdateLobby(string lobbyId, Lobby lobby)
//	{
//		CollectionReference lobbiesRef = firestore.Collection("Lobbies");

//		await lobbiesRef.Document(lobbyId).SetAsync(lobby);
//	}

//	public async Task UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp)
//	{
//		if (playerId is null || lobbyId is null) return;

//		CollectionReference lobbiesRef = firestore.Collection("Lobbies");

//		DocumentReference lobbyRef = lobbiesRef.Document(lobbyId);

//		try
//		{
//			await lobbyRef.UpdateAsync($"LobbyPlayersData.{playerId}.LastSeen", timestamp);
//		}
//		catch (Exception e)
//		{
//			Debug.Log($"Couldn't update lobby- player-lastseen: {e.Message}");
//		}
//	}

//	private async Task UpdateLobbyPlayerList(string lobbyId, List<string> playerIds)
//	{
//		CollectionReference lobbiesRef = firestore.Collection("Lobbies");

//		await lobbiesRef.Document(lobbyId).UpdateAsync("Players", playerIds);
//	}

//	private async Task<Dictionary<string, LobbyPlayerData>> GetLobbyPlayerData(string lobbyId)
//	{
//		CollectionReference lobbiesRef = firestore.Collection("Lobbies");

//		DocumentReference lobby = lobbiesRef.Document(lobbyId);

//		DocumentSnapshot snapshot = await lobby.GetSnapshotAsync();

//		Lobby lobby1 = snapshot.ConvertTo<Lobby>();

//		return lobby1?.LobbyPlayersData;
//	}

//	public async Task RemovePlayerFromLobby(string lobbyId, string playerId)
//	{
//		List<Player> players = await GetLobbyPlayers(lobbyId);

//		List<string> playerIds = players.Select((player) => player.Id).ToList();

//		playerIds.Remove(playerId);

//		await UpdateLobbyPlayerList(lobbyId, playerIds);

//		await RemoveLobbyPlayerData(lobbyId, playerId);
//	}

//	public async Task<List<Player>> GetLobbyPlayers(string lobbyId)
//	{
//		CollectionReference lobbyRef = firestore.Collection("Lobbies");

//		DocumentSnapshot snapshot = await lobbyRef.Document(lobbyId).GetSnapshotAsync();

//		Lobby lobby = snapshot.ConvertTo<Lobby>();

//		if (lobby is null) return null;

//		List<string> playerIds = lobby.Players;

//		List<Player> players = await GetAllPlayers(playerIds.ToArray());

//		return players;
//	}

//	public async Task UpdateLastSeen(string playerId, Timestamp timestamp)
//	{
//		CollectionReference playersRef = firestore.Collection("Players");

//		Player player = await GetPlayerById(playerId);

//		player.LastSeen = timestamp;

//		await playersRef.Document(playerId).SetAsync(player);
//	}

//	public async Task RemoveInactivePlayersFromLobby(string lobbyId, int secondsTimeout)
//	{
//		Dictionary<string, LobbyPlayerData> playersLastSeen = await GetLobbyPlayerData(lobbyId);

//		if (playersLastSeen is null) return;

//		foreach (var player in playersLastSeen)
//		{
//			double secondsSinceLastSeen = (Timestamp.GetCurrentTimestamp().ToDateTime() - player.Value.LastSeen.ToDateTime()).TotalSeconds;

//			if (secondsSinceLastSeen > secondsTimeout)
//			{
//				Debug.Log($"Removed player {player.Key} from lobby {lobbyId}");
//				await RemovePlayerFromLobby(lobbyId, player.Key);
//			}
//		}
//	}

//	public async Task RemoveLobbyPlayerData(string lobbyId, string playerId)
//	{
//		CollectionReference lobbiesRef = firestore.Collection("Lobbies");

//		await lobbiesRef.Document(lobbyId).UpdateAsync($"LobbyPlayersData.{playerId}", FieldValue.Delete);
//	}

//	public async Task UpdateLobbyMap(string lobbyId, string mapId)
//	{
//		CollectionReference lobbiesRef = firestore.Collection("Lobbies");

//		DocumentReference lobbyRef = lobbiesRef.Document(lobbyId);

//		await lobbyRef.UpdateAsync("MapId", mapId);
//	}

//	public async Task<string> GetLobbyMap(string lobbyId)
//	{
//		DocumentReference lobbyRef = firestore.Collection("Lobbies").Document(lobbyId);

//		DocumentSnapshot snapshot = await lobbyRef.GetSnapshotAsync();

//		Lobby lobby = snapshot.ConvertTo<Lobby>();

//		return lobby.MapId;
//	}

//	public async Task<string> GetLobbyHost(string lobbyId)
//	{
//		DocumentReference lobbyRef = firestore.Collection("Lobbies").Document(lobbyId);

//		DocumentSnapshot snapshot = await lobbyRef.GetSnapshotAsync();

//		Lobby lobby = snapshot.ConvertTo<Lobby>();

//		return lobby.HostId;
//	}

//	public async Task SetHost(string lobbyId, string playerId)
//	{
//		DocumentReference lobbyRef = firestore.Collection("Lobbies").Document(lobbyId);

//		await lobbyRef.UpdateAsync("HostId", playerId);
//	}

//	public async Task RemoveEmptyLobbies()
//	{
//		CollectionReference lobbiesRef = firestore.Collection("Lobbies");

//		QuerySnapshot snapshot = await lobbiesRef.GetSnapshotAsync();

//		foreach (DocumentSnapshot docSnapshot in snapshot.Documents)
//		{
//			Lobby lobby = docSnapshot.ConvertTo<Lobby>();

//			if (lobby.Players.Count <= 0)
//			{
//				await RemoveLobby(lobby.Id);
//			}
//		}
//	}

//	public async Task RemoveLobby(string lobbyId)
//	{
//		DocumentReference lobbyRef = firestore.Collection("Lobbies").Document(lobbyId);

//		await lobbyRef.DeleteAsync();
//	}

//	public async Task<Dictionary<string, PlayerColor>> GetLobbyColors(string lobbyId)
//	{
//		DocumentReference lobbyRef = firestore.Collection("Lobbies").Document(lobbyId);

//		DocumentSnapshot snapshot = await lobbyRef.GetSnapshotAsync();

//		Dictionary<string, LobbyPlayerData> lobbyPlayersData = snapshot.ConvertTo<Lobby>().LobbyPlayersData;

//		Dictionary<string, PlayerColor> result = new();

//		foreach (var item in lobbyPlayersData)
//		{
//			result.Add(item.Key, item.Value.Color);
//		}

//		return result;
//	}

//	public async Task<PlayerColor> GetPlayerColor(string lobbyId, string playerId)
//	{
//		CollectionReference lobbiesRef = firestore.Collection("Lobbies");

//		DocumentReference lobbyRef = lobbiesRef.Document(lobbyId);

//		DocumentSnapshot snapshot = await lobbyRef.GetSnapshotAsync();

//		Lobby lobby = snapshot.ConvertTo<Lobby>();

//		Dictionary<string, LobbyPlayerData> lobbyPlayersData = lobby.LobbyPlayersData;

//		if (lobbyPlayersData.ContainsKey(playerId))
//			return lobbyPlayersData[playerId].Color;
//		else return 0;
//	}

//	public async Task UpdatePlayerColor(string lobbyId, string playerId, PlayerColor color)
//	{
//		CollectionReference lobbiesRef = firestore.Collection("Lobbies");

//		DocumentReference lobbyRef = lobbiesRef.Document(lobbyId);

//		await lobbyRef.UpdateAsync($"LobbyPlayersData.{playerId}.Color", color);
//	}

//	//NEW

//	public IEnumerator GetLobbies(Action<List<Lobby>> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator RegisterPlayer(Player newPlayer, Action<Player> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator Login(Player newPlayer, Action<Player> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator PlayerExists(Player newPlayer, Action<bool> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetLatestMotd(Action<Motd> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetAllPlayers(Action<List<Player>> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetAllPlayers(string[] playerIds, Action<List<Player>> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator CreateLobby(Player host, string name, string description, string mapId, Action<Lobby> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetPlayerByName(string name, Action<Player> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetPlayerById(string id, Action<Player> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetLobby(string id, Action<Lobby> callback)
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.AddPlayerToLobby(string lobbyId, string playerId)
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp)
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.UpdateLastSeen(string playerId, Timestamp timestamp)
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.RemoveInactivePlayersFromLobby(string lobbyId, int secondsTimeout)
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.RemovePlayerFromLobby(string lobbyId, string playerId)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetLobbyPlayers(string lobbyId, Action<List<Player>> callback)
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.RemoveLobbyPlayerData(string lobbyId, string playerId)
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.UpdateLobbyMap(string lobbyId, string mapId)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetLobbyMap(string lobbyId, Action<string> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetLobbyHost(string lobbyId, Action<string> callback)
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.SetHost(string lobbyId, string playerId)
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.RemoveEmptyLobbies()
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.RemoveLobby(string lobbyId)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetLobbyColors(string lobbyId, Action<Dictionary<string, PlayerColor>> callback)
//	{
//		throw new NotImplementedException();
//	}

//	public IEnumerator GetPlayerColor(string lobbyId, string playerId, Action<PlayerColor> callback)
//	{
//		throw new NotImplementedException();
//	}

//	IEnumerator IDatabaseManager.UpdatePlayerColor(string lobbyId, string playerId, PlayerColor color)
//	{
//		throw new NotImplementedException();
//	}
//}