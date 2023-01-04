using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Threading.Tasks;
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


		var getImageTask = motdImageRef.GetBytesAsync(maxFileSize);

		getImageTask.Start();

		yield return new WaitUntil(() => getImageTask.IsCompleted);

		texture.LoadImage(getImageTask.Result);

		callback.Invoke(texture);
	}
}
