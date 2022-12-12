using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public GameObject mainMenuBackground;
	public GameObject accountMenuBackground;

	public GameObject mainMenu;
	public GameObject accountMenu;

	private GameObject[] menus;
	private GameObject[] backgrounds;

	private GameEventsManager gameEvents;

	private void Start()
	{
		gameEvents = GameEventsManager.instance;
		gameEvents.OnPlayerLoggedIn += PlayerLoggedIn;
		gameEvents.OnPlayerRegistered += PlayerRegistered;

		menus = new GameObject[]{
			mainMenu,
			accountMenu
		};

		backgrounds = new GameObject[]
		{
			mainMenuBackground,
			accountMenuBackground
		};
	}

	/// <summary>
	/// Navigates to a certain using the enum <paramref name="klootnMenu"/>
	/// </summary>
	/// <param name="klootnMenu"></param>
	public void NavigateTo(KlootnMenu klootnMenu)
	{
		GameObject objToNavigateTo = GetMenu(klootnMenu);

		foreach (GameObject obj in menus)
		{
			if (objToNavigateTo.Equals(obj))
				obj.SetActive(true);
			else
				obj.SetActive(false);
		}

		SwapBackground(klootnMenu);
	}

	/// <summary>
	/// Method that is subscribed to <see cref="GameEventsManager.OnPlayerRegistered"/>
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void PlayerRegistered(object sender, RegisterEventArgs e)
	{
		NavigateTo(KlootnMenu.MainMenu);
	}

	/// <summary>
	/// Method that is subscribed to <see cref="GameEventsManager.OnPlayerLoggedIn"/>
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void PlayerLoggedIn(object sender, LoginEventArgs e)
	{
		NavigateTo(KlootnMenu.MainMenu);
	}

	/// <summary>
	/// Activate the <see cref="GameObject"/> given by <paramref name="klootnMenu"/> and deactivates all other background <see cref="GameObject"/>s
	/// </summary>
	/// <param name="klootnMenu"></param>
	private void SwapBackground(KlootnMenu klootnMenu)
	{
		GameObject backgroundObj = GetBackground(klootnMenu);

		foreach (GameObject obj in backgrounds)
		{
			if (obj.Equals(backgroundObj)) obj.SetActive(true);
			else obj.SetActive(false);
		}
	}

	/// <summary>
	/// Gets the corresponding <see cref="GameObject"/> according to the given <paramref name="klootnMenu"/>
	/// </summary>
	/// <param name="klootnMenu"></param>
	/// <returns></returns>
	private GameObject GetBackground(KlootnMenu klootnMenu)
	{
		switch (klootnMenu)
		{
			case KlootnMenu.AccountMenu:
				return accountMenuBackground;
			case KlootnMenu.MainMenu:
				return mainMenuBackground;
			default:
				return mainMenuBackground;
		}
	}

	/// <summary>
	/// Gets the corresponding <see cref="GameObject"/> according to the given <paramref name="klootnMenu"/>
	/// </summary>
	/// <param name="klootnMenu"></param>
	/// <returns></returns>
	private GameObject GetMenu(KlootnMenu klootnMenu)
	{
		switch (klootnMenu)
		{
			case KlootnMenu.AccountMenu:
				return accountMenu;
			case KlootnMenu.MainMenu:
				return mainMenu;
			default:
				return accountMenu;
		}
	}

	/// <summary>
	/// Set (<see cref="GameObject"/>) <paramref name="obj"/> to (<see cref="bool"/>) <paramref name="active"/>
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="active"></param>
	public void ToggleMenu(GameObject obj)
	{
		obj.SetActive(!obj.activeSelf);
	}
}
