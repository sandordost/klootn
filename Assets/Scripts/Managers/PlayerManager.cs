using Firebase.Firestore;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataRecievable
{
	public float UpdateClientLastSeenInterval = 5;
	private Timer UpdateLastSeenTimer;
	private Player client;
	public Player Client 
	{ 
		get 
		{
			if (client != null)
				return client;
			else return null;

		}
		set
		{
			client = value;
		} 
	}

	private IDatabaseManager databaseManager;


	private void Start()
	{
		databaseManager = GameManager.GetInstance().dataManager.databaseManager;

		SetTestClient();
		UpdateLastSeenTimer = new Timer(UpdateClientLastSeenInterval * 1000);
		UpdateLastSeenTimer.Elapsed += UpdateLastSeenTimer_Elapsed;
		UpdateLastSeenTimer.Start();
	}

	private void UpdateLastSeenTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		UpdateLastSeen();
	}

	private IEnumerator UpdateLastSeen()
	{
		Client.LastSeen = Timestamp.GetCurrentTimestamp();
		yield return databaseManager.UpdateLastSeen(Client.Id, Client.LastSeen);
	}

	public IEnumerator GetPlayer(string playerId, Action<Player> callback)
	{
		yield return databaseManager.GetPlayerById(playerId, (player) => 
		{
			callback.Invoke(player);
		});
	}

	public IEnumerator RetrieveData()
	{
		throw new System.NotImplementedException();
	}

	private IEnumerator SetTestClient()
	{
		yield return databaseManager.GetPlayerByName("sandor", (player) => 
		{
			Client = player;
		});
	}
}
