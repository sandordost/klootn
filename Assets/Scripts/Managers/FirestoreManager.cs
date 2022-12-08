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
		CollectionReference docRef = firestore.Collection("Players");

		docRef.AddAsync(newPlayer).ContinueWithOnMainThread((task) =>
		{
		});

		yield return GetPlayers((callback) => { });
	}

	public IEnumerator GetPlayers(Action<List<Player>> onCallBack)
	{
		CollectionReference docRef = firestore.Collection("Players");

		QuerySnapshot documentSnapshot;

		var collecting = docRef.GetSnapshotAsync().ContinueWithOnMainThread((task) => {
			documentSnapshot = task.Result;

			foreach(DocumentSnapshot snapshot in documentSnapshot.Documents)
			{
				NewPlayer player = snapshot.ConvertTo<NewPlayer>();
				Debug.Log($"{player.id} - {player.name} - {player.password}");
			}
		});
		yield return null;
	}
}