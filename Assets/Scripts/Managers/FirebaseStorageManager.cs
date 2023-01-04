using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseStorageManager : IStorageManager
{
	const long maxFileSize = 1 * 1024 * 1024;

	StorageReference mainStorageRef;

	public FirebaseStorageManager()
	{
		mainStorageRef = FirebaseStorage.DefaultInstance.RootReference;
	}

	public IEnumerator GetImage(string imageUrl, Action<Texture> callback)
	{
		//StorageReference motdImageRef = firebaseStorage.GetReferenceFromUrl(imageUrl);

		//Texture2D texture = new Texture2D(1, 1);


		//var getImageTask = motdImageRef.GetBytesAsync(maxFileSize);

		//getImageTask.Start();

		//yield return new WaitUntil(() => getImageTask.IsCompleted);

		//texture.LoadImage(getImageTask.Result);

		//callback.Invoke(texture);
		yield return null;
	}
}
