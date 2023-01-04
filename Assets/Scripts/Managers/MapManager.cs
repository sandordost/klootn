using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.Collections;

public class MapManager : MonoBehaviour, IDataRecievable
{
	public KlootnMap[] maps;

	private IDatabaseManager databaseManager;

	public string DefaultMap { 
		get 
		{
			if (maps.Length > 0)
				return maps[0].id;
			else return null;
		}
	}

	private void Start()
	{
		databaseManager = GameManager.GetInstance().dataManager.databaseManager;
	}

	public KlootnMap[] GetKlootnMaps()
	{
		return maps;
	}

	public IEnumerator RetrieveData()
	{
		throw new NotImplementedException();
	}

	public IEnumerator UpdateMapId(string lobbyId, string mapId)
	{
		yield return databaseManager.UpdateLobbyMap(lobbyId, mapId);
	}

	public IEnumerator GetCurrentKlootnMap(string lobbyId, Action<KlootnMap> callback)
	{
		string mapId = null;
		yield return databaseManager.GetLobbyMap(lobbyId, (dbMapId) =>
		{
			mapId = dbMapId;
		});

		if (mapId is null)
			callback.Invoke(null);


		callback.Invoke(maps.FirstOrDefault((map) => map.id.Equals(mapId)));
	}

	public KlootnMap GetMap(string mapId)
	{
		foreach (KlootnMap map in maps)
			if (map.id.Equals(mapId))
				return map;

			return null;
	}
}