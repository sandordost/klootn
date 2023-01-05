using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyClick : MonoBehaviour
{
	public string LobbyId { get; set; }

    private Button buttonComponent;

	private UIManager uiManager;

	private LobbyManagerUI lobbyManagerUI;

	private InLobbyManagerUI inLobbyManagerUI;

	private void Start()
	{
		uiManager = UIManager.GetInstance();

		lobbyManagerUI = uiManager.lobbyManagerUI;

		buttonComponent = GetComponent<Button>();

		buttonComponent.onClick.AddListener(new UnityAction(ButtonClicked));
	}

	private void ButtonClicked()
	{
		lobbyManagerUI.StartOpenLobbyUI(LobbyId);
	}
}
