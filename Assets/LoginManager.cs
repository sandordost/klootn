using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
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

    public void LogIn()
	{
        NewPlayer newPlayer = new NewPlayer(usernameInputField.text, passwordInputField.text);
        StartCoroutine(databaseManager.Login(newPlayer, (player) =>
        {
            PlayerLoggedIn(player);
        }));
	}

    public void PlayerLoggedIn(Player player)
	{
        if (player != null)
        {
            gameEvents.LoginPlayer(this, player);
            Debug.Log($"{player.name} - {player.id}");
        }
        else Debug.Log($"Player has not been found or password is incorrect");
	}
}
