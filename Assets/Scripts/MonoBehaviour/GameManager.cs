using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	public DataManager dataManager;
	public SceneManager sceneManager;
	public SettingsManager settingsManager;

	private void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
		sceneManager.LoadScene(KlootnScene.MainMenu);
	}

	/// <summary>
	/// Returns the singleton instance of <see cref="GameManager"/>
	/// </summary>
	/// <returns></returns>
	public static GameManager GetGameManager()
	{
		return instance;
	}
}
