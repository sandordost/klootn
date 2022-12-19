using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirestoreManager : IDatabaseManager
{
	FirebaseFirestore firestore;

	public FirestoreManager()
	{
		firestore = FirebaseFirestore.DefaultInstance;
	}

	public async Task<Motd> GetLatestMotd()
	{
		CollectionReference collectionReference = firestore.Collection("Motds");

		var task = await collectionReference.OrderByDescending("Uploaddate").GetSnapshotAsync();

		QuerySnapshot result = task;

		Motd motd = null;

		if (result.Count > 0)
			motd = result[0].ConvertTo<Motd>();

		return motd;
	}

	public async Task<List<Lobby>> GetLobbies()
	{
		CollectionReference colRef = firestore.Collection("Lobbies");

		QuerySnapshot snapshot = await colRef.GetSnapshotAsync();

		List<Lobby> result = new();

		foreach(var item in snapshot.Documents)
		{
			result.Add(item.ConvertTo<Lobby>());
		}

		return result;
	}

	public async Task<Lobby> CreateLobby(Player host, string name, string description)
	{
		Lobby lobby = new()
		{
			Host = host,
			Name = name,
			Description = description
		};

		lobby.Players.Add(host);

		CollectionReference colRef = firestore.Collection("Lobbies");

		DocumentReference newDocument = await colRef.AddAsync(lobby);

		lobby.Id = newDocument.Id;

		return lobby;
	}

	public async Task<Player> RegisterPlayer(NewPlayer newPlayer)
	{
		CollectionReference colRef = firestore.Collection("Players");

		DocumentReference addedPlayer = await colRef.AddAsync(newPlayer);

		DocumentReference docRef = addedPlayer;

		Player result = await GetPlayerById(docRef.Id);

		return result;
	}

	public async Task<Player> Login(NewPlayer newPlayer)
	{
		CollectionReference playersRef = firestore.Collection("Players");

		Query query = playersRef.WhereEqualTo("name", newPlayer.name);
		query = query.WhereEqualTo("password", newPlayer.password);

		var task = await query.GetSnapshotAsync();

		var result = task;

		Player playerResult = null;

		if (result.Count > 0)
			playerResult = result[0].ConvertTo<Player>();

		return playerResult;
	}

	public async Task<bool> PlayerExists(NewPlayer newPlayer)
	{
		Player player = await GetPlayerByName(newPlayer.name);

		return player != null;
	}

	public async Task<List<Player>> GetPlayers()
	{
		CollectionReference playersRef = firestore.Collection("Players");

		QuerySnapshot snapshot = await playersRef.GetSnapshotAsync();

		List<Player> result = new();

		foreach (var item in snapshot.Documents)
		{
			result.Add(item.ConvertTo<Player>());
		}

		return result;
	}

	private async Task<Player> GetPlayerById(string id)
	{
		CollectionReference playersRef = firestore.Collection("Players");

		Query query = playersRef.WhereEqualTo(FieldPath.DocumentId, id);

		var task = await query.GetSnapshotAsync();

		QuerySnapshot result = task;

		Player player = null;

		if (result.Count > 0)
			player = result[0].ConvertTo<Player>();

		return player;
	}

	private async Task<Player> GetPlayerByName(string name)
	{
		CollectionReference playersRef = firestore.Collection("Players");

		Query query = playersRef.WhereEqualTo("name", name);

		var task = await query.GetSnapshotAsync();

		QuerySnapshot result = task;

		Player player = null;

		if (result.Count > 0)
			player = result[0].ConvertTo<Player>();

		return player;
	}
}