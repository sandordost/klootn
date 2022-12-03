using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public SceneManager SceneManager { get; set; }
	public SettingsManager SettingsManager { get; set; }

	public GameManager()
	{
		SceneManager = new SceneManager();
		SettingsManager = new SettingsManager();
	}

	void Start()
    {
        DontDestroyOnLoad(gameObject);
		SceneManager.LoadScene(KlootnScene.MainMenu);
    }
}
