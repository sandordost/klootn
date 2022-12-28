using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_UI : MonoBehaviour
{
	private InLobbyManagerUI inLobbyManagerUI;

	public string PlayerId { get; set; }

	private void Start()
	{
		inLobbyManagerUI = UIManager.GetInstance().inLobbyManagerUI;
	}

	public void KickPlayer()
	{
		inLobbyManagerUI.KickPlayer(PlayerId);
		Debug.Log($"Kicked player {PlayerId}");
	}

	public void PromotePlayer()
	{
		inLobbyManagerUI.PromotePlayer(PlayerId);
		Debug.Log($"Promote player {PlayerId} to host");
	}
}
