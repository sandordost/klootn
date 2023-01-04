
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SQLDatabaseManager : IDatabaseManager
{
	private const string klootnUrl = "https://sandordost.nl/klootn/database/";
	
	public IEnumerator AddPlayerToLobby(string lobbyId, string playerId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator CreateLobby(Player host, string name, string description, string mapId, Action<Lobby> callback)
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

	public IEnumerator GetLatestMotd(Action<Motd> callback)
	{
		UnityWebRequest webrequest = new UnityWebRequest("https://sandordost.nl/klootn/database/motd.php", "GET");
		webrequest.downloadHandler = new DownloadHandlerBuffer();
		yield return webrequest.SendWebRequest();

		if (webrequest.result.Equals(UnityWebRequest.Result.Success))
		{
			//Convert JSON to object
			var result = webrequest.downloadHandler.text;

			Motd motd = JsonUtility.FromJson<Motd>(result);

			callback.Invoke(motd);
		}

		
	}

	public IEnumerator GetLobbies(Action<List<Lobby>> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobby(string id, Action<Lobby> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobbyColors(string lobbyId, Action<Dictionary<string, PlayerColor>> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobbyHost(string lobbyId, Action<string> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobbyMap(string lobbyId, Action<string> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobbyPlayers(string lobbyId, Action<List<Player>> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetPlayerById(string id, Action<Player> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetPlayerByName(string name, Action<Player> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetPlayerColor(string lobbyId, string playerId, Action<PlayerColor> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator Login(Player newPlayer, Action<Player> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator PlayerExists(Player newPlayer, Action<bool> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator RegisterPlayer(Player newPlayer, Action<Player> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator RemoveEmptyLobbies()
	{
		throw new NotImplementedException();
	}

	public IEnumerator RemoveInactivePlayersFromLobby(string lobbyId, int secondsTimeout)
	{
		throw new NotImplementedException();
	}

	public IEnumerator RemoveLobby(string lobbyId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator RemoveLobbyPlayerData(string lobbyId, string playerId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator RemovePlayerFromLobby(string lobbyId, string playerId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator SetHost(string lobbyId, string playerId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator UpdateLastSeen(string playerId, Timestamp timestamp)
	{
		throw new NotImplementedException();
	}

	public IEnumerator UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp)
	{
		throw new NotImplementedException();
	}

	public IEnumerator UpdateLobbyMap(string lobbyId, string mapId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator UpdatePlayerColor(string lobbyId, string playerId, PlayerColor color)
	{
		throw new NotImplementedException();
	}
}
