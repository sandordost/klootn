using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	public IDatabaseManager databaseManager;
	public IStorageManager storageManager;

	public PlayerManager playerManager;
	public LobbyManager lobbyManager;
	public GameCommandsManager gameCommandsManager;
	public GameStateManager gameStateManager;
	public MapManager mapManager;

	public DatabaseOption databaseOption;
	public StorageOption storageOption;

	private void Awake()
	{
		SetDatabaseManager(databaseOption);
		SetStorageOption(storageOption);
	}

	/// <summary>
	/// Sets the <see cref="IStorageManager"/> using the given <paramref name="option"/>
	/// </summary>
	/// <param name="option"></param>
	private void SetStorageOption(StorageOption option)
	{
		switch (option)
		{
			case StorageOption.FirebaseStorage:
				storageManager = new FirebaseStorageManager();
				break;
			case StorageOption.WebStorage:
				storageManager = new WebStorageManager();
				break;
		}
	}

	/// <summary>
	/// Sets the <see cref="IDatabaseManager"/> using the <paramref name="option"/>.
	/// </summary>
	/// <param name="option"></param>
	private void SetDatabaseManager(DatabaseOption option)
	{
		switch (option)
		{
			case DatabaseOption.Firebase:
				//databaseManager = new FirebaseManager();
				break;
			case DatabaseOption.SQLDatabase:
				databaseManager = new SQLDatabaseManager();
				break;
			case DatabaseOption.Firestore:
				//databaseManager = new FirestoreManager();
				break;
		}
	}
}
