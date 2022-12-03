using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private IDatabaseManager databaseManager;
    public PlayerManager playerManager;
    public LobbyManager lobbyManager;
    public GameCommandsManager gameCommandsManager;
    public GameStateManager gameStateManager;

	public DataManager()
	{
        databaseManager = new SQLDatabaseManager();
	}

	private void Start()
	{
		
	}
}
