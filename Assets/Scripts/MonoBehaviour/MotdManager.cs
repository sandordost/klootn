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

	

	private void Start()
	{
		databaseManager = GameManager.GetGameManager().dataManager.databaseManager;

		SetUIComponents();
	}

	private void SetUIComponents()
	{
		StartCoroutine(GetLatestMotd((motd) =>
		{
			title.text = motd.Title;
			message.text = motd.Message;

			motdImage.texture = GetMotdImage(motd.ImageUrl);
		}));
		
	}

	private Texture GetMotdImage(string imgUrl)
	{
		return null;
	}

	private IEnumerator GetLatestMotd(Action<Motd> callback)
	{
		yield return databaseManager.GetLatestMotd((motd) => 
		{
			callback.Invoke(motd);
		});
	}
}
