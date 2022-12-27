using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectionManager : MonoBehaviour
{
	private MapManager mapManager;

	private InLobbyManagerUI inLobbyManagerUI;

    public GameObject mapPrefab;

    public GameObject mapPrefabParent;


	private void Start()
	{
		inLobbyManagerUI = UIManager.GetInstance().inLobbyManagerUI;
		GameManager gameManager = GameManager.GetInstance();
		mapManager = gameManager.dataManager.mapManager;
		LoadMapUI();
	}

	private void LoadMapUI()
	{
		KlootnMap[] maps = mapManager.GetKlootnMaps();

        foreach(KlootnMap map in maps)
		{
			GameObject newMapPrefab = Instantiate(mapPrefab, mapPrefabParent.transform);

			newMapPrefab.transform.Find("Image").GetComponent<Image>().sprite = map.image;

			newMapPrefab.transform.Find("MapInfo").Find("MapName").GetComponent<TMP_Text>().text = map.title;

			newMapPrefab.transform.Find("MapInfo").Find("MapDescription").GetComponent<TMP_Text>().text = map.description;

			newMapPrefab.GetComponent<MapUIIdentifier>().mapId = map.id;
		}
	}

	public void SelectMap(string mapId)
	{
		mapManager.UpdateMapId(inLobbyManagerUI.CurrentLobbyId, mapId);
	}
}
