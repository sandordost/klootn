using System;
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
		databaseManager = GameManager.GetGameManager().dataManager.databaseManager;

		SetTestClient();
		UpdateLastSeenTimer = new Timer(UpdateClientLastSeenInterval * 1000);
		UpdateLastSeenTimer.Elapsed += UpdateLastSeenTimer_Elapsed;
		UpdateLastSeenTimer.Start();
	}

	private void UpdateLastSeenTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		UpdateLastSeen();
	}

	private async void UpdateLastSeen()
	{
		Client.LastSeen = DateTime.Now;
		await databaseManager.UpdateLastSeen(Client.Id, Client.LastSeen);
	}

	public async Task<Player> GetPlayer(string playerId)
	{
		return await databaseManager.GetPlayerById(playerId);
	}

	public Task RetrieveData()
	{
		throw new System.NotImplementedException();
	}

	private async void SetTestClient()
	{
		Client = await GameManager.GetGameManager().dataManager.databaseManager.GetPlayerByName("sandordost");
	}
}
