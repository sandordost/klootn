using System.Diagnostics;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
	public TMP_InputField usernameInputField;
	public TMP_InputField passwordInputField;

	public TMP_Text usernameErrorMessage;
	public TMP_Text passwordErrorMessage;
	public TMP_Text overalErrorMessage;

	private IDatabaseManager databaseManager;
	private PlayerManager playerManager;

	private GameEventsManager gameEvents;

	private InputValidator inputValidator = new InputValidator();

	public int maxLoginAttempts = 10;
	public int secondsCooldown = 60;

	private int loginAttempts = 0;
	private Stopwatch loginCooldownTimer = new Stopwatch();

	private void Start()
	{
		GameManager gameManager = GameManager.GetGameManager();

		usernameInputField.characterLimit = inputValidator.maxPlayerNameSize;
		passwordInputField.characterLimit = inputValidator.maxPasswordSize;

		databaseManager = gameManager.dataManager.databaseManager;
		playerManager = gameManager.dataManager.playerManager;

		gameEvents = GameEventsManager.instance;

		usernameErrorMessage.gameObject.SetActive(false);
		passwordErrorMessage.gameObject.SetActive(false);
		overalErrorMessage.gameObject.SetActive(false);
	}

	public void LogIn()
	{
		if (HasLoginAttempts())
		{
			TryLogin();
		}
		else
		{
			ShowErrorMessage($"Too many login attempts. You must wait ({secondsCooldown - loginCooldownTimer.Elapsed.Seconds}) seconds");
		}
	}

	private void TryLogin()
	{
		usernameErrorMessage.gameObject.SetActive(false);
		passwordErrorMessage.gameObject.SetActive(false);
		overalErrorMessage.gameObject.SetActive(false);

		NewPlayer newPlayer = new NewPlayer(usernameInputField.text, passwordInputField.text);

		ValidationResult nameResult = inputValidator.ValidatePlayerName(newPlayer.name);
		ValidationResult passwordResult = inputValidator.ValidatePlayerPassword(newPlayer.password);

		if (nameResult.Equals(ValidationResult.Validated) &&
			passwordResult.Equals(ValidationResult.Validated))
		{
			StartCoroutine(inputValidator.ValidatePlayerNameExists(newPlayer, databaseManager, (validationResult) =>
			{
				if (validationResult.Equals(ValidationResult.AlreadyExists))
				{
					StartCoroutine(databaseManager.Login(newPlayer, (player) =>
					{
						if (player != null)
						{
							PlayerLoggedIn(player);
						}
						else
						{
							ShowValidationError(nameResult, ValidationResult.PasswordIncorrect);
						}
					}));
				}
				else
				{
					ShowValidationError(ValidationResult.DoesNotExist, passwordResult);
				}
			}));
		}
		else
		{
			ShowValidationError(nameResult, passwordResult);
		}
	}

	private bool HasLoginAttempts()
	{
		if (loginAttempts < maxLoginAttempts)
		{
			loginAttempts++;
			return true;
		}

		if (loginCooldownTimer.IsRunning)
		{
			if (loginCooldownTimer.Elapsed.Seconds < secondsCooldown) return false;

			loginAttempts = 0;

			loginCooldownTimer.Stop();
			loginCooldownTimer.Reset();
		}
		else loginCooldownTimer.Start();

		return false;
	}

	private void ShowErrorMessage(string message)
	{
		overalErrorMessage.text = message;
		overalErrorMessage.gameObject.SetActive(true);
	}

	private void PlayerLoggedIn(Player player)
	{
		if (player != null)
		{
			playerManager.Client = player;
			gameEvents.LoginPlayer(this, player);
		}
	}

	private void ShowValidationError(ValidationResult nameResult, ValidationResult passwordResult)
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
