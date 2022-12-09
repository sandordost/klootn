using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

	public event EventHandler<LoginEventArgs> OnPlayerLoggedIn;
	public event EventHandler<RegisterEventArgs> OnPlayerRegistered;

	private void Awake()
	{
		instance = this;
	}

	public void RegisterPlayer(object sender, Player registeredPlayer)
	{
		RegisterEventArgs eventArgs = new RegisterEventArgs() { player = registeredPlayer };

		if(OnPlayerRegistered != null)
			OnPlayerRegistered.Invoke(sender, eventArgs);
	}

	public void LoginPlayer(object sender, Player loggedInPlayer)
	{
		LoginEventArgs eventArgs = new LoginEventArgs() { player = loggedInPlayer };

		if(OnPlayerLoggedIn != null)
			OnPlayerLoggedIn.Invoke(sender, eventArgs);
	}
}
