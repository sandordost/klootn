using UnityEngine;
using TMPro;
using System;
using System.Diagnostics;

public class RegistrationManager : MonoBehaviour
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

	public int maxRegisterAttempts = 10;
	public int secondsCooldown = 60;

	private int RegisterAttempts = 0;
	private Stopwatch registerCooldownTimer = new Stopwatch();

	private void Start()
	{
		usernameErrorMessage.gameObject.SetActive(false);
		passwordErrorMessage.gameObject.SetActive(false);
		overalErrorMessage.gameObject.SetActive(false);

		GameManager gameManager = GameManager.GetGameManager();

		databaseManager = gameManager.dataManager.databaseManager;
		playerManager = gameManager.dataManager.playerManager;

		gameEvents = GameEventsManager.instance;
	}

	public void RegisterPlayer()
	{
		if (HasRegisterAttempts())
		{
			usernameErrorMessage.gameObject.SetActive(false);
			passwordErrorMessage.gameObject.SetActive(false);
			overalErrorMessage.gameObject.SetActive(false);

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
		else
		{
			ShowErrorMessage($"Too many register attempts. You must wait ({secondsCooldown - registerCooldownTimer.Elapsed.Seconds}) seconds");
		}
	}

	private void ShowErrorMessage(string message)
	{
		overalErrorMessage.text = message;
		overalErrorMessage.gameObject.SetActive(true);
	}

	private bool HasRegisterAttempts()
	{
		if (RegisterAttempts < maxRegisterAttempts)
		{
			RegisterAttempts++;
			return true;
		}

		if (registerCooldownTimer.IsRunning)
		{
			if (registerCooldownTimer.Elapsed.Seconds < secondsCooldown) return false;

			RegisterAttempts = 0;

			registerCooldownTimer.Stop();
			registerCooldownTimer.Reset();
		}
		else registerCooldownTimer.Start();

		return false;
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
