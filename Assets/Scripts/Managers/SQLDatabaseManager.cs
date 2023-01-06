
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class SQLDatabaseManager : IDatabaseManager
{
	private const string klootnUrl = "https://sandordost.nl/klootn/database/";
	private const string motdsUrl = "motds.php";
	private const string playersUrl = "players.php";
	private const string lobbiesUrl = "lobbies.php";

	public IEnumerator AddPlayerToLobby(string lobbyId, string playerId)
	{
		WWWForm formdata = new WWWForm();
		formdata.AddField("LobbyId", lobbyId);
		formdata.AddField("PlayerId", playerId);

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "addplayertolobby", (result) => { });
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

		if(lobbyId != null)
		{
			yield return GetLobby(lobbyId, (lobby) =>
			{
				callback.Invoke(lobby);
			});
		}
	}

	public IEnumerator GetAllPlayers(Action<List<Player>> callback)
	{
		yield return SendGetRequest(klootnUrl + playersUrl, (result) =>
		{
			Player[] players = JsonConvert.DeserializeObject<Player[]>(result);
			callback.Invoke(new List<Player>(players));
		});
	}

	public IEnumerator GetAllPlayers(string[] playerIds, Action<List<Player>> callback)
	{
		WWWForm formdata = new WWWForm();
		string jsonstring = JsonConvert.SerializeObject(playerIds);
		formdata.AddField("PlayerIds", jsonstring);

		yield return SendPostRequest(klootnUrl + playersUrl, formdata, "getplayersbyid", (result) =>
		{
			if (!result.Equals("null"))
			{
				Player[] players = JsonConvert.DeserializeObject<Player[]>(result);
				callback.Invoke(new List<Player>(players));
			}
			else callback.Invoke(null);
		});
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
		yield return SendGetRequest(klootnUrl + lobbiesUrl, (result) =>
		{
			Lobby[] lobbies = JsonConvert.DeserializeObject<Lobby[]>(result);
			callback.Invoke(new List<Lobby>(lobbies));
		});
	}

	public IEnumerator GetLobby(string id, Action<Lobby> callback)
	{
		WWWForm formdata = new WWWForm();

		formdata.AddField("Id", id);

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "getlobbybyid", (result) =>
		{
			if (result.Length > 0)
				callback.Invoke(JsonUtility.FromJson<Lobby>(result));
			else callback.Invoke(null);
		});
	}

	public IEnumerator GetLobbyColors(string lobbyId, Action<Dictionary<string, PlayerColor>> callback)
	{
		WWWForm formdata = new WWWForm();
		formdata.AddField("LobbyId", lobbyId);

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "getallplayerdata", (result) =>
		{
			Dictionary<string, PlayerColor> colors = new Dictionary<string, PlayerColor>();

			LobbyPlayerData[] playerDatas = JsonConvert.DeserializeObject<LobbyPlayerData[]>(result);

			foreach(var item in playerDatas)
			{
				colors.Add(item.PlayerId, item.Color);
			}

			callback.Invoke(colors);
		});
	}

	public IEnumerator GetLobbyHost(string lobbyId, Action<string> callback)
	{
		WWWForm formdata = new WWWForm();
		formdata.AddField("Id", lobbyId);

		string hostId = null;

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "getlobbyhost", (result) =>
		{
			if (int.TryParse(result, out int parsedId))
			{
				hostId = parsedId.ToString();
			}
		});

		if (hostId != null)
			callback.Invoke(hostId);
	}

	public IEnumerator GetLobbyMap(string lobbyId, Action<string> callback)
	{
		WWWForm formdata = new WWWForm();
		formdata.AddField("Id", lobbyId);

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "getlobbymap", (result) =>
		{
			callback.Invoke(result);
		});
			
	}

	public IEnumerator GetLobbyPlayers(string lobbyId, Action<List<Player>> callback)
	{
		WWWForm formdata = new WWWForm();
		formdata.AddField("Id", lobbyId);

		string[] playerIds = null;
		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "getlobbyplayerids", (result) =>
		{
			playerIds = JsonConvert.DeserializeObject<string[]>(result);
		});

		yield return GetAllPlayers(playerIds, (result) =>
		{
			if (result != null)
			{
				callback.Invoke(result);
			}
			else callback.Invoke(new List<Player>());
		});
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
		WWWForm formdata = new WWWForm();
		formdata.AddField("LobbyId", lobbyId);
		formdata.AddField("PlayerId", playerId);

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "getplayerdata", (result) =>
		{
			LobbyPlayerData lpd = JsonConvert.DeserializeObject<LobbyPlayerData>(result);
			callback.Invoke(lpd.Color);
		});
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

		if (foundplayerId != null)
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
		yield return SendPostRequest(klootnUrl + lobbiesUrl, "removeemptylobbies", (result) => 
		{
			if (result.Length > 0)
				Debug.Log($"RemoveEmptyLobbies returned: {result}");
		});
	}

	public IEnumerator RemoveInactivePlayersFromLobby(string lobbyId, int secondsTimeout)
	{
		WWWForm formdata = new WWWForm();

		formdata.AddField("LobbyId", lobbyId);
		formdata.AddField("TimeoutTime", secondsTimeout);

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "removeinactiveplayers", (result) =>
		{
			if(result.Length > 0)
				Debug.Log($"RemoveInactivePlayersFromLobby returned: {result}");
		});
	}

	public IEnumerator RemoveLobby(string lobbyId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator RemoveLobbyPlayerData(string lobbyId, string playerId)
	{
		WWWForm formdata = new WWWForm();

		formdata.AddField("LobbyId", lobbyId);
		formdata.AddField("PlayerId", playerId);

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "removeplayerdata", (result) =>
		{
			Debug.Log($"kick player with id: {playerId}, response: {result}");
		});
	}

	public IEnumerator RemovePlayerFromLobby(string lobbyId, string playerId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator SetHost(string lobbyId, string playerId)
	{
		WWWForm formdata = new WWWForm();

		formdata.AddField("Id", lobbyId);
		formdata.AddField("HostId", playerId);

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "sethost", (result) => 
		{
			Debug.Log($"Made host of player with id: {playerId}, response: {result}");
		});

	}

	public IEnumerator UpdateLastSeen(string playerId, DateTime timestamp)
	{
		throw new NotImplementedException();
	}

	public IEnumerator UpdateLobbyLastSeen(string playerId, string lobbyId)
	{
		WWWForm formdata = new WWWForm();
		formdata.AddField("PlayerId", playerId);
		formdata.AddField("LobbyId", lobbyId);

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "setplayerlastseen", (result) =>
		{

		});
	}

	public IEnumerator UpdateLobbyMap(string lobbyId, string mapId)
	{
		throw new NotImplementedException();
	}

	public IEnumerator UpdatePlayerColor(string lobbyId, string playerId, PlayerColor color)
	{
		WWWForm formdata = new WWWForm();
		formdata.AddField("LobbyId", lobbyId);
		formdata.AddField("PlayerId", playerId);
		formdata.AddField("Color", (int)color);

		yield return SendPostRequest(klootnUrl + lobbiesUrl, formdata, "setplayercolor", (result) =>
		{

		});
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

		webrequest.Dispose();
	}
	private IEnumerator SendPostRequest(string url, WWWForm formdata, Action<string> callback)
	{
		UnityWebRequest webrequest = UnityWebRequest.Post(url, formdata);

		yield return webrequest.SendWebRequest();

		if (webrequest.result.Equals(UnityWebRequest.Result.Success))
		{
			callback.Invoke(webrequest.downloadHandler.text);
		}

		webrequest.Dispose();
	}
	private IEnumerator SendPostRequest(string url, WWWForm formdata, string action, Action<string> callback)
	{
		formdata.AddField("action", action);
		yield return SendPostRequest(url, formdata, (result) =>
		{
			callback.Invoke(result);
		});
	}
	private IEnumerator SendPostRequest(string url, string action, Action<string> callback)
	{
		WWWForm formdata = new WWWForm();
		formdata.AddField("action", action);
		yield return SendPostRequest(url, formdata, (result) =>
		{
			callback.Invoke(result);
		});
	}
}
