using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
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
        GameManager gameManager = GameManager.GetGameManager();

        databaseManager = gameManager.dataManager.databaseManager;
        playerManager = gameManager.dataManager.playerManager;

        gameEvents = GameEventsManager.instance;

        usernameErrorMessage.gameObject.SetActive(false);
        passwordErrorMessage.gameObject.SetActive(false);
    }

    public void LogIn()
	{
        usernameErrorMessage.gameObject.SetActive(false);
        passwordErrorMessage.gameObject.SetActive(false);

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
