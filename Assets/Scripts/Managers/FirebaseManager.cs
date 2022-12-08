using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FirebaseManager : IDatabaseManager
{
	DatabaseReference databaseReference;

	public FirebaseManager()
	{
		databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	public IEnumerator RegisterPlayer(NewPlayer newPlayer, Action<Player> onCallback)
	{
		int playerId = 0;

		yield return GetHighestId("Players", (int id) => 
		{
			playerId = id + 1;
		});

		yield return PushObject(newPlayer, "Players", playerId.ToString());

		Player result = new Player(newPlayer.name, playerId.ToString());

		onCallback.Invoke(result);
	}

	private IEnumerator PushObject(object obj, string table, string id)
	{
		var task = databaseReference.Child(table).Child(id).SetRawJsonValueAsync(JsonUtility.ToJson(obj));

		yield return new WaitUntil(() => task.IsCompleted);
	}

	private IEnumerator GetHighestId(string table, Action<int> onCallback)
	{
		var response = databaseReference.Child(table).GetValueAsync();

		yield return new WaitUntil(() => response.IsCompleted);

		if (response != null)
		{
			try
			{
				IEnumerable<DataSnapshot> dataSnapshots = response.Result.Children;

				int highestFound = 0;
				foreach(DataSnapshot snapshot in dataSnapshots)
				{
					int result = int.Parse(snapshot.Value.ToString());
					if (result > highestFound)
						highestFound = result;
				}

				onCallback.Invoke(highestFound);
			}
			catch(Exception ex)
			{
				Debug.Log($"Problem has occured: {ex.Message}");
				onCallback.Invoke(0);
			}
		}
		else
		{
			onCallback.Invoke(0);
		}
	}
}
