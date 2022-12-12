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

	public async Task<Texture> GetImage(Motd motd)
	{
		StorageReference motdImageRef = firebaseStorage.GetReferenceFromUrl(motd.ImageUrl);

		Texture2D texture = new Texture2D(1, 1);

		var result = await motdImageRef.GetBytesAsync(maxFileSize);

		texture.LoadImage(result);

		return texture;
	}
}
