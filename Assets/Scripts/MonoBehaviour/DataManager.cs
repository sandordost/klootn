using UnityEngine;

public class DataManager : MonoBehaviour
{
	public IDatabaseManager databaseManager;
	public IStorageManager storageManager;

	public PlayerManager playerManager;
	public LobbyManager lobbyManager;
	public GameCommandsManager gameCommandsManager;
	public GameStateManager gameStateManager;

	public DatabaseOption databaseOption;
	public StorageOption storageOption;

	private void Awake()
	{
		SetDatabaseManager(databaseOption);
		SetStorageOption(StorageOption.FirebaseStorage);
	}

	private void SetStorageOption(StorageOption option)
	{
		switch (option)
		{
			case StorageOption.FirebaseStorage:
				storageManager = new FirebaseStorageManager();
				break;
		}
	}

	private void SetDatabaseManager(DatabaseOption option)
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
