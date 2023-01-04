using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MotdManager : MonoBehaviour
{
	public TMP_Text title;
	public TMP_Text message;
	public RawImage motdImage;
	public RawImage altImage;

	private IDatabaseManager databaseManager;
	private IStorageManager storageManager;

	private void Start()
	{
		GameManager gameManager = GameManager.GetInstance();

		databaseManager = gameManager.dataManager.databaseManager;
		storageManager = gameManager.dataManager.storageManager;

		SetUIComponents();
	}

	private IEnumerator SetUIComponents()
	{
		Motd motd = null;
		yield return GetLatestMotd((dbMotd) => 
		{
			motd = dbMotd;
			title.text = motd.Title;
			message.text = motd.Message;
		});

		yield return GetMotdImage(motd, (text) => 
		{
			motdImage.texture = text;

			motdImage.color = Color.white;
			altImage.gameObject.SetActive(false);
		});
	}

	/// <summary>
	/// Gets the <see cref="Texture"/> from the <see cref="databaseManager"/> using the <see cref="Motd.ImageUrl"/> from <paramref name="motd"/>.
	/// </summary>
	/// <param name="motd"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	private IEnumerator GetMotdImage(Motd motd, Action<Texture> callback)
	{
		yield return storageManager.GetImage(motd, (tex) =>
		{
			callback.Invoke(tex);
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
