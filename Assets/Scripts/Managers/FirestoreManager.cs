using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class FirestoreManager : IDatabaseManager
{
	FirebaseFirestore firestore;

	public FirestoreManager()
	{
		firestore = FirebaseFirestore.DefaultInstance;
	}

	public IEnumerator RegisterPlayer(NewPlayer newPlayer, Action<Player> callback)
	{
		CollectionReference colRef = firestore.Collection("Players");

		var addplayerTask = colRef.AddAsync(newPlayer);

		yield return new WaitUntil(() => addplayerTask.IsCompleted);
		
		DocumentReference docRef = addplayerTask.Result;

		Player result = null;

		yield return GetPlayerById(docRef.Id, (player) =>
		{
			result = player;
		});

		callback.Invoke(result);
	}

	public IEnumerator GetPlayers(Action<List<Player>> callBack)
	{
		List<Player> playersResult = new();

		CollectionReference docRef = firestore.Collection("Players");

		var collecting = docRef.GetSnapshotAsync();

		yield return new WaitUntil(() => collecting.IsCompleted);

		QuerySnapshot snapshot = collecting.Result;

		foreach(var item in snapshot.Documents)
		{
			Player newPlayer = item.ConvertTo<Player>();

			playersResult.Add(newPlayer);
		}

		callBack.Invoke(playersResult);
	}

	private IEnumerator GetPlayerById(string id, Action<Player> callback)
	{
		CollectionReference playersRef = firestore.Collection("Players");

		Query query = playersRef.WhereEqualTo(FieldPath.DocumentId, id);

		var task = query.GetSnapshotAsync();

		yield return new WaitUntil(() => task.IsCompleted);

		QuerySnapshot result = task.Result;

		Player player = null;

		if (result.Count > 0)
			player = result[0].ConvertTo<Player>();

		callback.Invoke(player);
	}

	private IEnumerator GetPlayerByName(string name, Action<Player> callback)
	{
		CollectionReference playersRef = firestore.Collection("Players");

		Query query = playersRef.WhereEqualTo("name", name);

		var task = query.GetSnapshotAsync();

		yield return new WaitUntil(() => task.IsCompleted);

		QuerySnapshot result = task.Result;

		Player player = result[0].ConvertTo<Player>();

		callback.Invoke(player);
	}

	public IEnumerator Login(NewPlayer newPlayer, Action<Player> callback)
	{
		CollectionReference playersRef = firestore.Collection("Players");

		Query query = playersRef.WhereEqualTo("name", newPlayer.name);
		query = query.WhereEqualTo("password", newPlayer.password);

		var task = query.GetSnapshotAsync();

		yield return new WaitUntil(() => task.IsCompleted);

		var result = task.Result;

		Player playerResult = null;

		if (result.Count > 0)
			playerResult = result[0].ConvertTo<Player>();

		callback.Invoke(playerResult);
	}
}