using System.Threading.Tasks;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataRecievable
{
	public Player Client { get; set; }

	public Task RetrieveData()
	{
		throw new System.NotImplementedException();
	}
}
