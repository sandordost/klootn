using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public DataManager dataManager;
	public SceneManager sceneManager;
	public SettingsManager settingsManager;

	public GameManager()
	{
		sceneManager = new SceneManager();
		settingsManager = new SettingsManager();
	}

	void Start()
    {
        DontDestroyOnLoad(gameObject);
		sceneManager.LoadScene(KlootnScene.MainMenu);
    }
}
