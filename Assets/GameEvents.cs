using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

	public event EventHandler<LoginEventArgs> PlayerLoggedIn;
	public event EventHandler<RegisterEventArgs> PlayerRegistered;

	private void Awake()
	{
		instance = this;
	}

	public void RegisterPlayer(object sender, Player registeredPlayer)
	{
		RegisterEventArgs eventArgs = new RegisterEventArgs() { player = registeredPlayer };

		PlayerRegistered.Invoke(sender, eventArgs);
	}

	public void LoginPlayer(object sender, Player loggedInPlayer)
	{
		LoginEventArgs eventArgs = new LoginEventArgs() { player = loggedInPlayer };

		PlayerLoggedIn.Invoke(sender, eventArgs);
	}
}
