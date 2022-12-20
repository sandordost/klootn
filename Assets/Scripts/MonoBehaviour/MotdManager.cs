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
		GameManager gameManager = GameManager.GetGameManager();

		databaseManager = gameManager.dataManager.databaseManager;
		storageManager = gameManager.dataManager.storageManager;

		SetUIComponents();
	}

	private async void SetUIComponents()
	{
		Motd motd = await GetLatestMotd();

		title.text = motd.Title;
		message.text = motd.Message;

		Texture texture = await GetMotdImage(motd);
		motdImage.texture = texture;

		motdImage.color = Color.white;
		altImage.gameObject.SetActive(false);
	}

	/// <summary>
	/// Gets the <see cref="Texture"/> from the <see cref="databaseManager"/> using the <see cref="Motd.ImageUrl"/> from <paramref name="motd"/>.
	/// </summary>
	/// <param name="motd"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	private async Task<Texture> GetMotdImage(Motd motd)
	{
		return await storageManager.GetImage(motd);
	}

	/// <summary>
	/// Gets the latest <see cref="Motd"/> from the <see cref="databaseManager"/>. 
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	private async Task<Motd> GetLatestMotd()
	{
		return await databaseManager.GetLatestMotd();
	}
}
