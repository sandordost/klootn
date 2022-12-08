using UnityEngine;
using TMPro;

public class RegistrationManager : MonoBehaviour
{
	public TMP_InputField usernameInputField;
	public TMP_InputField passwordInputField;
	private IDatabaseManager databaseManager;
	private GameEvents gameEvents;

	private void Start()
	{
		databaseManager = GameManager.GetGameManager().dataManager.databaseManager;
		gameEvents = GameEvents.instance;
	}

	public void RegisterPlayer()
	{
		NewPlayer newPlayer = new NewPlayer(usernameInputField.text, "0", passwordInputField.text);

		StartCoroutine(databaseManager.RegisterPlayer(newPlayer, (player) =>
		{
			gameEvents.RegisterPlayer(this, player);
		}));
	}
}
