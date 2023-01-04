using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebStorageManager : IStorageManager
{
	const long maxFileSize = 1 * 1024 * 1024;

	public IEnumerator GetImage(string imageUrl, Action<Texture> callback)
	{
		UnityWebRequest webrequest = new UnityWebRequest(imageUrl, "GET");

		webrequest.downloadHandler = new DownloadHandlerBuffer();

		yield return webrequest.SendWebRequest();

		if (webrequest.result.Equals(UnityWebRequest.Result.Success))
		{
			//Create texture
			byte[] data = webrequest.downloadHandler.data;

			Texture2D texture = new Texture2D(1,1);

			texture.LoadImage(data);

			callback.Invoke(texture);
		}
	}
}