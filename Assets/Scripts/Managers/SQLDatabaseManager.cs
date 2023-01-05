
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
	private const string motdsUrl = "motds.php";
	private const string playersUrl = "players.php";
	private const string lobbiesUrl = "lobbies.php";

	public IEnumerator AddPlayerToLobby(string lobbyId, string playerId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator CreateLobby(Player host, string name, string description, string mapId, Action<Lobby> callback)
	{
		WWWForm formdata = new WWWForm();

		formdata.AddField("Name", name);
		formdata.AddField("Description", description);
		formdata.AddField("HostId", host.Id);
		formdata.AddField("MapId", mapId);

		string lobbyId = null;
		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "createlobby", (result) =>
		{
			if (int.TryParse(result, out int parsedId))
			{
				lobbyId = parsedId.ToString();
			}
		});

		if(lobbyId is not null)
		{
			yield return GetLobby(lobbyId, (lobby) =>
			{
				callback.Invoke(lobby);
			});
		}
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
		string jsonString = "";
		yield return SendGetRequest(klootnUrl + motdsUrl, (jsonstring) =>
		{
			jsonString = jsonstring;
		});

		Motd motd = JsonUtility.FromJson<Motd>(jsonString);
		callback.Invoke(motd);
	}

	public IEnumerator GetLobbies(Action<List<Lobby>> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator GetLobby(string id, Action<Lobby> callback)
	{
		WWWForm formdata = new WWWForm();

		formdata.AddField("Id", id);

		yield return SendPostRequest(klootnUrl + playersUrl, formdata, "getlobbybyid", (result) =>
		{
			if (result.Length > 0)
				callback.Invoke(JsonUtility.FromJson<Lobby>(result));
			else callback.Invoke(null);
		});
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
		WWWForm formdata = new WWWForm();

		formdata.AddField("Id", id);

		yield return SendPostRequest(klootnUrl + playersUrl, formdata, "getplayerbyid", (result) =>
		{
			if (result.Length > 0)
				callback.Invoke(JsonUtility.FromJson<Player>(result));
			else callback.Invoke(null);
		});
	}

	public IEnumerator GetPlayerByName(string name, Action<Player> callback)
	{
		WWWForm formdata = new WWWForm();

		formdata.AddField("Name", name);

		yield return SendPostRequest(klootnUrl + playersUrl, formdata, "getplayerbyname", (result) =>
		{
			if (result.Length > 0)
				callback.Invoke(JsonUtility.FromJson<Player>(result));
			else callback.Invoke(null);
		});
	}

	public IEnumerator GetPlayerColor(string lobbyId, string playerId, Action<PlayerColor> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator Login(Player newPlayer, Action<Player> callback)
	{
		WWWForm formdata = new WWWForm();

		formdata.AddField("Name", newPlayer.Name);
		formdata.AddField("Password", newPlayer.Password);

		string foundplayerId = null;
		yield return SendPostRequest(klootnUrl + playersUrl, formdata, "login", (playerId) =>
		{
			if(playerId.Length > 0)
				foundplayerId = playerId;
		});

		if (foundplayerId is not null)
		{
			yield return GetPlayerById(foundplayerId, (player) =>
			{
				callback.Invoke(player);
			});
		}
	}

	public IEnumerator PlayerExists(Player newPlayer, Action<bool> callback)
	{
		WWWForm formdata = new WWWForm();

		formdata.AddField("Name", newPlayer.Name);

		yield return SendPostRequest(klootnUrl + playersUrl, formdata, "checkplayerexists", (reply) =>
		{
			callback.Invoke(reply.Equals("1"));
		});
	}

	public IEnumerator RegisterPlayer(Player newPlayer, Action<Player> callback)
	{
		WWWForm formdata = new WWWForm();

		formdata.AddField("Name", newPlayer.Name);
		formdata.AddField("Password", newPlayer.Password);

		string createdPlayerId = null;
		yield return SendPostRequest(klootnUrl + playersUrl, formdata, "register", (reply) =>
		{
			if(int.TryParse(reply, out int parsedId))
			{
				createdPlayerId = parsedId.ToString();
			}
		});

		yield return GetPlayerById(createdPlayerId, (player) =>
		{
			callback.Invoke(player);
		});
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

	private IEnumerator SendGetRequest(string url, Action<string> callback)
	{
		UnityWebRequest webrequest = new UnityWebRequest(url, "GET");

		webrequest.downloadHandler = new DownloadHandlerBuffer();

		yield return webrequest.SendWebRequest();

		if (webrequest.result.Equals(UnityWebRequest.Result.Success))
		{
			var result = webrequest.downloadHandler.text;
			callback.Invoke(result);
		}
	}

	private IEnumerator SendPostRequest(string url, WWWForm formdata, Action<string> callback)
	{
		UnityWebRequest webrequest = UnityWebRequest.Post(url, formdata);

		yield return webrequest.SendWebRequest();

		if (webrequest.result.Equals(UnityWebRequest.Result.Success))
		{
			callback.Invoke(webrequest.downloadHandler.text);
		}
	}

	private IEnumerator SendPostRequest(string url, WWWForm formdata, string action, Action<string> callback)
	{
		formdata.AddField("action", action);
		yield return SendPostRequest(url, formdata, (result) =>
		{
			callback.Invoke(result);
		});
	}

	private IEnumerator SendPostRequest(string url, object jsonData, Action<string> callback)
	{
		string jsonString = JsonUtility.ToJson(jsonData);

		UnityWebRequest webrequest = UnityWebRequest.Post(url, jsonString);

		webrequest.SetRequestHeader("Content-Type", "application/json");

		yield return webrequest.SendWebRequest();

		if (webrequest.result.Equals(UnityWebRequest.Result.Success))
		{
			var result = webrequest.downloadHandler.text;
			callback.Invoke(result);
		}
	}
}
