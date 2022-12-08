using System.Collections;
using System.Collections.Generic;
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

	public static GameManager GetGameManager()
	{
		return instance;
	}
}
