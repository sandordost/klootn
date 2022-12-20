using Firebase.Database;
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
	DatabaseReference databaseReference;

	public FirebaseManager()
	{
		databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	public async Task<Player> RegisterPlayer(NewPlayer newPlayer)
	{
		int playerId = await GetHighestId("Players");

		playerId++;

		await PushObject(newPlayer, "Players", playerId.ToString());

		return new Player(newPlayer.Name, playerId.ToString());
	}

	private async Task PushObject(object obj, string table, string id)
	{
		await databaseReference.Child(table).Child(id).SetRawJsonValueAsync(JsonUtility.ToJson(obj));
	}

	private async Task<int> GetHighestId(string table)
	{
		var response = await databaseReference.Child(table).GetValueAsync();

		if (response != null)
		{
			try
			{
				var dataSnapshots = response.Children;

				int highestFound = 0;
				foreach (DataSnapshot snapshot in dataSnapshots)
				{
					int result = int.Parse(snapshot.Value.ToString());
					if (result > highestFound)
						highestFound = result;
				}

				return highestFound;
			}
			catch (Exception ex)
			{
				Debug.Log($"Problem has occured: {ex.Message}");
				return 0;
			}
		}
		else
		{
			return 0;
		}
	}

	public IEnumerator PlayerExists(NewPlayer newPlayer, Action<bool> callback)
	{
		throw new NotImplementedException();
	}

	public Task<Motd> GetLatestMotd()
	{
		throw new NotImplementedException();
	}

	public Task<Player> Login(NewPlayer newPlayer)
	{
		throw new NotImplementedException();
	}

	public Task<bool> PlayerExists(NewPlayer newPlayer)
	{
		throw new NotImplementedException();
	}

	public Task<List<Lobby>> GetLobbies()
	{
		throw new NotImplementedException();
	}

	public Task<List<Player>> GetPlayers()
	{
		throw new NotImplementedException();
	}

	public Task<Lobby> CreateLobby(Player host, string title, string description)
	{
		throw new NotImplementedException();
	}

	public Task<Player> GetPlayerByName(string name)
	{
		throw new NotImplementedException();
	}

	public Task<Lobby> GetLobby(string id)
	{
		throw new NotImplementedException();
	}
}
