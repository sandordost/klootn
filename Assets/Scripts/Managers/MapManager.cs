using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

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

	public Task RetrieveData()
	{
		throw new NotImplementedException();
	}

	public async void UpdateMapId(string lobbyId, string mapId)
	{
		await databaseManager.UpdateLobbyMap(lobbyId, mapId);
	}

	public async Task<KlootnMap> GetCurrentKlootnMap(string lobbyId)
	{
		string mapId = await databaseManager.GetLobbyMap(lobbyId);

		return maps.FirstOrDefault((map) => map.id.Equals(mapId));
	}

	public KlootnMap GetMap(string mapId)
	{
		foreach (KlootnMap map in maps)
			if (map.id.Equals(mapId))
				return map;

			return null;
	}
}