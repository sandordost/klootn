using Firebase.Database;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Implements <see cref="IDatabaseManager"/>. Manages the data from the <b>Google Firebase Database</b>
/// </summary>
public class FirebaseManager : IDatabaseManager
{
	//DatabaseReference databaseReference;

	//public FirebaseManager()
	//{
	//	databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
	//}

	//public async Task<Player> RegisterPlayer(Player newPlayer)
	//{
	//	int playerId = await GetHighestId("Players");

	//	playerId++;

	//	await PushObject(newPlayer, "Players", playerId.ToString());

	//	return new Player(newPlayer.Name, playerId.ToString());
	//}

	//private async Task PushObject(object obj, string table, string id)
	//{
	//	await databaseReference.Child(table).Child(id).SetRawJsonValueAsync(JsonUtility.ToJson(obj));
	//}

	//private async Task<int> GetHighestId(string table)
	//{
	//	var response = await databaseReference.Child(table).GetValueAsync();

	//	if (response != null)
	//	{
	//		try
	//		{
	//			var dataSnapshots = response.Children;

	//			int highestFound = 0;
	//			foreach (DataSnapshot snapshot in dataSnapshots)
	//			{
	//				int result = int.Parse(snapshot.Value.ToString());
	//				if (result > highestFound)
	//					highestFound = result;
	//			}

	//			return highestFound;
	//		}
	//		catch (Exception ex)
	//		{
	//			Debug.Log($"Problem has occured: {ex.Message}");
	//			return 0;
	//		}
	//	}
	//	else
	//	{
	//		return 0;
	//	}
	//}

	//public IEnumerator PlayerExists(Player newPlayer, Action<bool> callback)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<Motd> GetLatestMotd()
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<Player> Login(Player newPlayer)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<bool> PlayerExists(Player newPlayer)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<List<Lobby>> GetLobbies()
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<List<Player>> GetAllPlayers()
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<Lobby> CreateLobby(Player host, string title, string description)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<Player> GetPlayerByName(string name)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<Lobby> GetLobby(string id)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task AddPlayerToLobby(string lobbyId, string playerId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task UpdateLastSeen(string playerId, Timestamp timestamp)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<Player> GetPlayerById(string id)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task RemoveInactivePlayersFromLobby(string lobbyId, int secondsTimeout)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task RemovePlayerFromLobby(string lobbyId, string playerId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<List<Player>> GetLobbyPlayers(string lobbyId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task RemoveLobbyPlayerData(string lobbyId, string playerId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<List<Player>> GetAllPlayers(string[] playerIds)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task UpdateLobbyMap(string lobbyId, string mapId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<string> GetLobbyMap(string lobbyId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<Lobby> CreateLobby(Player host, string name, string description, string mapId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<string> GetLobbyHost(string lobbyId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task SetHost(string lobbyId, string playerId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task RemoveEmptyLobbies()
	//{
	//	throw new NotImplementedException();
	//}

	//public Task RemoveLobby(string lobbyId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<Dictionary<string, PlayerColor>> GetLobbyColors(string lobbyId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task<PlayerColor> GetPlayerColor(string lobbyId, string playerId)
	//{
	//	throw new NotImplementedException();
	//}

	//public Task UpdatePlayerColor(string lobbyId, string playerId, PlayerColor color)
	//{
	//	throw new NotImplementedException();
	//}

	public IEnumerator GetLobbies(Action<List<Lobby>> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator RegisterPlayer(Player newPlayer, Action<Player> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator Login(Player newPlayer, Action<Player> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLatestMotd(Action<Motd> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetAllPlayers(Action<List<Player>> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetAllPlayers(string[] playerIds, Action<List<Player>> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator CreateLobby(Player host, string name, string description, string mapId, Action<Lobby> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetPlayerByName(string name, Action<Player> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetPlayerById(string id, Action<Player> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobby(string id, Action<Lobby> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator AddPlayerToLobby(string lobbyId, string playerId)
	{
		throw new NotImplementedException();
	}

	IEnumerator IDatabaseManager.UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp)
	{
		throw new NotImplementedException();
	}

	IEnumerator IDatabaseManager.UpdateLastSeen(string playerId, Timestamp timestamp)
	{
		throw new NotImplementedException();
	}

	IEnumerator IDatabaseManager.RemoveInactivePlayersFromLobby(string lobbyId, int secondsTimeout)
	{
		throw new NotImplementedException();
	}

	IEnumerator IDatabaseManager.RemovePlayerFromLobby(string lobbyId, string playerId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobbyPlayers(string lobbyId, Action<List<Player>> callback)
	{
		throw new NotImplementedException();
	}

	IEnumerator IDatabaseManager.RemoveLobbyPlayerData(string lobbyId, string playerId)
	{
		throw new NotImplementedException();
	}

	IEnumerator IDatabaseManager.UpdateLobbyMap(string lobbyId, string mapId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobbyMap(string lobbyId, Action<string> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobbyHost(string lobbyId, Action<string> callback)
	{
		throw new NotImplementedException();
	}

	IEnumerator IDatabaseManager.SetHost(string lobbyId, string playerId)
	{
		throw new NotImplementedException();
	}

	IEnumerator IDatabaseManager.RemoveEmptyLobbies()
	{
		throw new NotImplementedException();
	}

	IEnumerator IDatabaseManager.RemoveLobby(string lobbyId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobbyColors(string lobbyId, Action<Dictionary<string, PlayerColor>> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetPlayerColor(string lobbyId, string playerId, Action<PlayerColor> callback)
	{
		throw new NotImplementedException();
	}

	IEnumerator IDatabaseManager.UpdatePlayerColor(string lobbyId, string playerId, PlayerColor color)
	{
		throw new NotImplementedException();
	}

	public IEnumerator PlayerExists(Player newPlayer, Action<bool> callback)
	{
		throw new NotImplementedException();
	}
}
