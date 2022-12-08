using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public IDatabaseManager databaseManager;
    public PlayerManager playerManager;
    public LobbyManager lobbyManager;
    public GameCommandsManager gameCommandsManager;
    public GameStateManager gameStateManager;
	public DatabaseOption databaseOption;
	
	private void Awake()
	{
		SetDatabaseManager(databaseOption);
	}

	public void SetDatabaseManager(DatabaseOption option)
	{
		switch (option)
		{
			case DatabaseOption.Firebase:
				databaseManager = new FirebaseManager();
				break;
			case DatabaseOption.SQLDatabase:
				databaseManager = new SQLDatabaseManager();
				break;
			case DatabaseOption.Firestore:
				databaseManager = new FirestoreManager();
				break;
		}
	}
}
