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

	public async Task<Player> RegisterPlayer(NewPlayer newPlayer)
	{
		CollectionReference colRef = firestore.Collection("Players");

		var addplayerTask = await colRef.AddAsync(newPlayer);

		DocumentReference docRef = addplayerTask;

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

	private IEnumerator GetPlayers(Action<List<Player>> callBack)
	{
		List<Player> playersResult = new();

		CollectionReference docRef = firestore.Collection("Players");

		var collecting = docRef.GetSnapshotAsync();

		yield return new WaitUntil(() => collecting.IsCompleted);

		QuerySnapshot snapshot = collecting.Result;

		foreach (var item in snapshot.Documents)
		{
			Player newPlayer = item.ConvertTo<Player>();

			playersResult.Add(newPlayer);
		}

		callBack.Invoke(playersResult);
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