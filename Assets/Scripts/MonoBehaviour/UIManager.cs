using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	private static UIManager instance;

	public LobbyManagerUI lobbyManagerUI;

	public InLobbyManagerUI inLobbyManagerUI;

	public MapSelectionManager mapSelectionManager;

	public UIPageSwitcher uiPageSwitcher;

	private void Awake()
	{
		instance = this;
	}

	public static UIManager GetInstance()
	{
		return instance;
	}
}
