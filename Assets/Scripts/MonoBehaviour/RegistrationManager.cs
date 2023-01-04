using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;

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
	private AlertManager alertManager;

	public int maxRegisterAttempts = 10;
	public int secondsCooldown = 60;

	private int registerAttempts = 0;
	private Stopwatch registerCooldownTimer = new Stopwatch();

	private void Start()
	{
		usernameErrorMessage.gameObject.SetActive(false);
		passwordErrorMessage.gameObject.SetActive(false);
		overalErrorMessage.gameObject.SetActive(false);
		alertManager = UIManager.GetInstance().alertManager;

		usernameInputField.characterLimit = inputValidator.maxPlayerNameSize;
		passwordInputField.characterLimit = inputValidator.maxPasswordSize;

		GameManager gameManager = GameManager.GetInstance();

		databaseManager = gameManager.dataManager.databaseManager;
		playerManager = gameManager.dataManager.playerManager;

		gameEvents = GameEventsManager.GetInstance();
	}

	/// <summary>
	/// If the player has <see cref="registerAttempts"/>, this method will do <see cref="TryRegister"/>. Else <see cref="ShowErrorMessage(string)"/>
	/// </summary>
	public void RegisterPlayer()
	{
		if (HasRegisterAttempts())
		{
			TryRegister();
		}
		else
		{
			ShowErrorMessage($"Too many register attempts. You must wait ({secondsCooldown - registerCooldownTimer.Elapsed.Seconds}) seconds");
		}
	}

	/// <summary>
	/// Attempt to register using "<see cref="usernameInputField"/>" and "<see cref="passwordInputField"/>" as input data
	/// </summary>
	private IEnumerator TryRegister()
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
			ValidationResult validationResult = ValidationResult.DoesNotExist;
			yield return inputValidator.ValidatePlayerNameExists(newPlayer, databaseManager, (result) =>
			{
				validationResult = result;
			});

			if (validationResult == ValidationResult.Exists)
				ShowValidationError(validationResult, passwordResult);
			else
			{

				Player player = null;
				yield return databaseManager.RegisterPlayer(newPlayer, (newplayer) =>
				{
					player = newplayer;
				});
				PlayerRegistered(player);
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
	/// Sets the <see cref="TMP_InputField.text"/> property to <paramref name="message"/> of (<see cref="TMP_InputField"/>) <see cref="overalErrorMessage"/>
	/// </summary>
	/// <param name="message"></param>
	private void ShowErrorMessage(string message)
	{
		overalErrorMessage.text = message;
		overalErrorMessage.gameObject.SetActive(true);
	}

	/// <summary>
	/// Checks whether the player has <see cref="registerAttempts"/>. 
	/// <para>
	/// Uses <see cref="registerCooldownTimer"/> to add a cooldown of: <see cref="secondsCooldown"/> 
	/// </para>
	/// </summary>
	/// <returns><b>True</b>: if <see cref="registerAttempts"/> are available. Else <b>False</b></returns>
	private bool HasRegisterAttempts()
	{
		if (registerAttempts < maxRegisterAttempts)
		{
			registerAttempts++;
			return true;
		}

		if (registerCooldownTimer.IsRunning)
		{
			if (registerCooldownTimer.Elapsed.Seconds < secondsCooldown) return false;

			registerAttempts = 0;

			registerCooldownTimer.Stop();
			registerCooldownTimer.Reset();
		}
		else registerCooldownTimer.Start();

		return false;
	}

	/// <summary>
	/// Sets <see cref="PlayerManager.Client"/> to the player and triggers the <see cref="GameEventsManager.OnPlayerLoggedIn"/>
	/// </summary>
	/// <param name="player"></param>
	private void PlayerRegistered(Player player)
	{
		playerManager.Client = player;
		gameEvents.RegisterPlayer(this, player);
		alertManager.ShowMessageAlert("Succesfuly registered", $"You have succesfully registered with the name \"{player.Name}\"");
	}
}
