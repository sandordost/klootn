using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	public DataManager dataManager;
	public SceneManager sceneManager;
	public SettingsManager settingsManager;

	public int standardFramerate;

	private void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
		sceneManager.LoadScene(KlootnScene.MainMenu);

		ChangeFramerate(standardFramerate);
	}

	/// <summary>
	/// Returns the singleton instance of <see cref="GameManager"/>
	/// </summary>
	/// <returns></returns>
	public static GameManager GetInstance()
	{
		return instance;
	}

	private void ChangeFramerate(int framerate)
	{
		Application.targetFrameRate = framerate;
	}
}
