using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUIIdentifier : MonoBehaviour
{
	public string mapId { get; set; }

	private Button buttonComponent;

	private UIManager uiManager;

	private MapSelectionManager mapSelectionManager;

	private UIPageSwitcher uiPageSwitcher;

	private void Awake()
	{
		buttonComponent = GetComponent<Button>();

		buttonComponent.onClick.AddListener(MapClicked);
	}

	private void Start()
	{
		uiManager = UIManager.GetInstance();

		mapSelectionManager = uiManager.mapSelectionManager;

		uiPageSwitcher = uiManager.uiPageSwitcher;
	}

	private void MapClicked()
	{
		Debug.Log($"Click on map: {mapId}");
		mapSelectionManager.SelectMap(mapId);
		uiPageSwitcher.SwitchPage("InLobbyPage");
	}
}
