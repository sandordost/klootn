using Firebase.Storage;
using System;
using System.Collections;
using UnityEngine;

public class FirebaseStorageManager : IStorageManager
{
	const long maxFileSize = 1 * 1024 * 1024;

	FirebaseStorage firebaseStorage;
	StorageReference mainStorageRef;

	public FirebaseStorageManager()
	{
		firebaseStorage = FirebaseStorage.DefaultInstance;
		mainStorageRef = firebaseStorage.RootReference;
	}

	public IEnumerator GetImage(Motd motd, Action<Texture> callback)
	{
		StorageReference motdImageRef = firebaseStorage.GetReferenceFromUrl(motd.ImageUrl);

		Texture2D texture = new Texture2D(1, 1);

		var task = motdImageRef.GetBytesAsync(maxFileSize);

		yield return new WaitUntil(() => task.IsCompleted);

		if (task.IsFaulted || task.IsCanceled)
		{
			Debug.LogException(task.Exception);
		}
		else
		{
			byte[] fileContents = task.Result;

			texture.LoadImage(fileContents);
		}

		callback.Invoke(texture);
	}
}
