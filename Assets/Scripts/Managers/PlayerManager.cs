using System.Threading.Tasks;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataRecievable
{
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

	private void Start()
	{
		SetTestClient();
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
