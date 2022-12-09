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

	private void PlayerRegistered(object sender, RegisterEventArgs e)
	{
		NavigateTo(KlootnMenu.MainMenu);
	}

	private void PlayerLoggedIn(object sender, LoginEventArgs e)
	{
		NavigateTo(KlootnMenu.MainMenu);
	}

	private void SwapBackground(KlootnMenu klootnMenu)
	{
		GameObject backgroundObj = GetBackground(klootnMenu);

		foreach (GameObject obj in backgrounds)
		{
			if (obj.Equals(backgroundObj)) obj.SetActive(true);
			else obj.SetActive(false);
		}
	}

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
}
