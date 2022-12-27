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
		GameManager gameManager = GameManager.GetInstance();

		usernameInputField.characterLimit = inputValidator.maxPlayerNameSize;
		passwordInputField.characterLimit = inputValidator.maxPasswordSize;

		databaseManager = gameManager.dataManager.databaseManager;
		playerManager = gameManager.dataManager.playerManager;

		gameEvents = GameEventsManager.GetInstance();

		usernameErrorMessage.gameObject.SetActive(false);
		passwordErrorMessage.gameObject.SetActive(false);
		overalErrorMessage.gameObject.SetActive(false);
	}

	/// <summary>
	/// If the player has <see cref="loginAttempts"/>, this method will do <see cref="TryLogin"/>. Else <see cref="ShowErrorMessage(string)"/>
	/// </summary>
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

	/// <summary>
	/// Attempt to login using "<see cref="usernameInputField"/>" and "<see cref="passwordInputField"/>" as input data
	/// </summary>
	private async void TryLogin()
	{
		usernameErrorMessage.gameObject.SetActive(false);
		passwordErrorMessage.gameObject.SetActive(false);
		overalErrorMessage.gameObject.SetActive(false);

		Player newPlayer = new Player(usernameInputField.text, new KlootnPassword(passwordInputField.text));

		ValidationResult nameResult = inputValidator.ValidatePlayerName(newPlayer.Name);
		ValidationResult passwordResult = inputValidator.ValidatePlayerPassword(newPlayer.Password);

		if (nameResult.Equals(ValidationResult.Validated) &&
			passwordResult.Equals(ValidationResult.Validated))
		{
			ValidationResult validationResult = await inputValidator.ValidatePlayerNameExists(newPlayer, databaseManager);
			if (validationResult.Equals(ValidationResult.AlreadyExists))
			{
				Player player = await databaseManager.Login(newPlayer);
				if (player != null)
				{
					PlayerLoggedIn(player);
				}
				else
				{
					ShowValidationError(nameResult, ValidationResult.PasswordIncorrect);
				}
			}
			else
			{
				ShowValidationError(ValidationResult.DoesNotExist, passwordResult);
			}
		}
		else
		{
			ShowValidationError(nameResult, passwordResult);
		}
	}

	/// <summary>
	/// Uses <see cref="inputValidator"/> to set <see cref="usernameInputField"/> and <see cref="passwordInputField"/> to the correct error message
	/// </summary>
	/// <param name="nameResult"></param>
	/// <param name="passwordResult"></param>
	private void ShowValidationError(ValidationResult nameResult, ValidationResult passwordResult)
	{
		inputValidator.ShowValidationError(nameResult, passwordResult, usernameErrorMessage, passwordErrorMessage);
	}

	/// <summary>
	/// Checks whether the player has <see cref="loginAttempts"/>. 
	/// <para>
	/// Uses <see cref="loginCooldownTimer"/> to add a cooldown of: <see cref="secondsCooldown"/> 
	/// </para>
	/// </summary>
	/// <returns><b>True</b>: if <see cref="loginAttempts"/> are available. Else <b>False</b></returns>
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

	/// <summary>
	/// Sets the <see cref="TMP_InputField.text"/> property to <paramref name="message"/> of (<see cref="TMP_InputField"/>) <see cref="overalErrorMessage"/>
	/// </summary>
	/// <param name="message"></param>
	private void ShowErrorMessage(string message)
	{
		overalErrorMessage.text = message;
		overalErrorMessage.gameObject.SetActive(true);
	}

	/// <summary>
	/// Sets <see cref="PlayerManager.Client"/> to the player and triggers the <see cref="GameEventsManager.OnPlayerLoggedIn"/>
	/// </summary>
	/// <param name="player"></param>
	private void PlayerLoggedIn(Player player)
	{
		if (player != null)
		{
			playerManager.Client = player;
			gameEvents.LoginPlayer(this, player);
		}
	}
}
