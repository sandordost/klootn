using UnityEngine;
using TMPro;

public class RegistrationManager : MonoBehaviour
{
	public TMP_InputField usernameInputField;
	public TMP_InputField passwordInputField;
	private IDatabaseManager databaseManager;

	private void Start()
	{
		databaseManager = GameManager.GetGameManager().dataManager.databaseManager;
	}

	public void RegisterPlayer()
	{
		NewPlayer player = new NewPlayer(usernameInputField.text, "0", passwordInputField.text);

		StartCoroutine(databaseManager.RegisterPlayer(player, (newPlayer) =>
		{
			PlayerRegistered(newPlayer);
		}));
	}

	public void PlayerRegistered(Player player)
	{
		Debug.Log($"{player.id} + {player.name}");
	}
}
