using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MotdManager : MonoBehaviour
{
	public TMP_Text title;
	public TMP_Text message;
	public RawImage motdImage;

	private IDatabaseManager databaseManager;
	private IStorageManager storageManager;

	private void Start()
	{
		GameManager gameManager = GameManager.GetGameManager();

		databaseManager = gameManager.dataManager.databaseManager;
		storageManager = gameManager.dataManager.storageManager;

		SetUIComponents();
	}

	private void SetUIComponents()
	{
		StartCoroutine(GetLatestMotd((motd) =>
		{
			title.text = motd.Title;
			message.text = motd.Message;

			StartCoroutine(GetMotdImage(motd, (texture) =>
			{
				motdImage.texture = texture;
			}));
		}));

	}

	/// <summary>
	/// Gets the <see cref="Texture"/> from the <see cref="databaseManager"/> using the <see cref="Motd.ImageUrl"/> from <paramref name="motd"/>.
	/// </summary>
	/// <param name="motd"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	private IEnumerator GetMotdImage(Motd motd, Action<Texture> callback)
	{
		yield return storageManager.GetImage(motd, (texture) =>
		{
			callback.Invoke(texture);
		});
	}

	/// <summary>
	/// Gets the latest <see cref="Motd"/> from the <see cref="databaseManager"/>. 
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	private IEnumerator GetLatestMotd(Action<Motd> callback)
	{
		yield return databaseManager.GetLatestMotd((motd) =>
		{
			callback.Invoke(motd);
		});
	}
}
