using UnityEngine;
using TMPro;

public class RegistrationManager : MonoBehaviour
{
	public TMP_InputField usernameInputField;
	public TMP_InputField passwordInputField;

	public TMP_Text usernameErrorMessage;
	public TMP_Text passwordErrorMessage;

	private IDatabaseManager databaseManager;
	private PlayerManager playerManager;

	private GameEventsManager gameEvents;
	private InputValidator inputValidator = new InputValidator();

	private void Start()
	{
		usernameErrorMessage.gameObject.SetActive(false);
		passwordErrorMessage.gameObject.SetActive(false);

		GameManager gameManager = GameManager.GetGameManager();

		databaseManager = gameManager.dataManager.databaseManager;
		playerManager = gameManager.dataManager.playerManager;

		gameEvents = GameEventsManager.instance;
	}

	public void RegisterPlayer()
	{
		usernameErrorMessage.gameObject.SetActive(false);
		passwordErrorMessage.gameObject.SetActive(false);

		NewPlayer newPlayer = new NewPlayer(usernameInputField.text, "0", passwordInputField.text);

		ValidationResult nameResult = inputValidator.ValidatePlayerName(newPlayer.name);
		ValidationResult passwordResult = inputValidator.ValidatePlayerPassword(newPlayer.password);

		if (nameResult.Equals(ValidationResult.Validated) &&
			passwordResult.Equals(ValidationResult.Validated))
		{
			StartCoroutine(inputValidator.ValidatePlayerNameExists(newPlayer, databaseManager, (validationResult) =>
			{
				if (validationResult == ValidationResult.AlreadyExists)
					ShowValidationError(validationResult, passwordResult);
				else
				{
					StartCoroutine(databaseManager.RegisterPlayer(newPlayer, (player) =>
					{
						RegisterPlayer(player);
					}));
				}
			}));

		}
		else
		{
			ShowValidationError(nameResult, passwordResult);
		}
	}

	private void RegisterPlayer(Player player)
	{
		playerManager.Client = player;
		gameEvents.RegisterPlayer(this, player);
	}

	public void ShowValidationError(ValidationResult nameResult, ValidationResult passwordResult)
	{
		string usernameString = inputValidator.GetUsernameErrorMessage(nameResult);
		if (usernameString != null)
		{
			usernameErrorMessage.gameObject.SetActive(true);
			usernameErrorMessage.text = usernameString;
		}

		string passwordString = inputValidator.GetPasswordErrorMessage(passwordResult);
		if (passwordString != null)
		{
			passwordErrorMessage.gameObject.SetActive(true);
			passwordErrorMessage.text = passwordString;
		}
	}
}
