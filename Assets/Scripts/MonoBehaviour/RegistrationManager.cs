using UnityEngine;
using TMPro;

public class RegistrationManager : MonoBehaviour
{
	public TMP_InputField usernameInputField;
	public TMP_InputField passwordInputField;

	public TMP_Text usernameErrorMessage;
	public TMP_Text passwordErrorMessage;

	private IDatabaseManager databaseManager;
	private GameEventsManager gameEvents;
	private InputValidator inputValidator = new InputValidator();

	private void Start()
	{
		usernameErrorMessage.gameObject.SetActive(false);
		passwordErrorMessage.gameObject.SetActive(false);

		databaseManager = GameManager.GetGameManager().dataManager.databaseManager;

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
			StartCoroutine(databaseManager.RegisterPlayer(newPlayer, (player) =>
			{
				gameEvents.RegisterPlayer(this, player);
			}));
		}
		else
		{
			ShowValidationError(nameResult, passwordResult);
		}
	}

	public void ShowValidationError(ValidationResult nameResult, ValidationResult passwordResult)
	{
		string usernameString = GetUsernameErrorMessage(nameResult);
		if(usernameString != null)
		{
			usernameErrorMessage.gameObject.SetActive(true);
			usernameErrorMessage.text = usernameString;
		}

		string passwordString = GetPasswordErrorMessage(passwordResult);
		if(passwordString != null)
		{
			passwordErrorMessage.gameObject.SetActive(true);
			passwordErrorMessage.text = passwordString;
		}
	}

	private string GetUsernameErrorMessage(ValidationResult validationResult)
	{
		return validationResult switch
		{
			ValidationResult.ShortStringLength =>
				$"Username is shorter than {inputValidator.minPlayerNameSize} characters",
			ValidationResult.LargeStringLength =>
				$"Username is larger than {inputValidator.maxPlayerNameSize} characters",
			ValidationResult.UnusableCharacters =>
				$"Username contains unusable characters",
			ValidationResult.Validated => null,
			_ => null,
		};
	}

	private string GetPasswordErrorMessage(ValidationResult validationResult)
	{
		return validationResult switch
		{
			ValidationResult.ShortStringLength =>
				$"Password is shorter than {inputValidator.minPasswordSize} characters",
			ValidationResult.LargeStringLength =>
				$"Password is larger than {inputValidator.maxPasswordSize} characters",
			ValidationResult.UnusableCharacters =>
				$"Password contains unusable characters",
			ValidationResult.Validated => null,
			_ => null
		};
	}
}
