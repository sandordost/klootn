using System;
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

	/// <summary>
	/// The event that gets triggered once the client registers a new account
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="registeredPlayer"></param>
	public void RegisterPlayer(object sender, Player registeredPlayer)
	{
		RegisterEventArgs eventArgs = new RegisterEventArgs() { player = registeredPlayer };

		if (OnPlayerRegistered != null)
			OnPlayerRegistered.Invoke(sender, eventArgs);
	}

	/// <summary>
	/// The events that triggers once the client logs in
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="loggedInPlayer"></param>
	public void LoginPlayer(object sender, Player loggedInPlayer)
	{
		LoginEventArgs eventArgs = new LoginEventArgs() { player = loggedInPlayer };

		if (OnPlayerLoggedIn != null)
			OnPlayerLoggedIn.Invoke(sender, eventArgs);
	}
}
