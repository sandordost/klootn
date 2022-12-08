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

	public IEnumerator RegisterPlayer(NewPlayer newPlayer, Action<Player> onCallback)
	{
		CollectionReference colRef = firestore.Collection("Players");

		var addplayerTask = colRef.AddAsync(newPlayer);

		yield return new WaitUntil(() => addplayerTask.IsCompleted);
		//TODO: Get id after or before insertion
		DocumentReference docRef = addplayerTask.Result;

		onCallback.Invoke(docRef.Id)
	}

	public IEnumerator GetPlayers(Action<List<Player>> onCallBack)
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

		onCallBack.Invoke(playersResult);
	}

	public IEnumerator GetPlayer()

}