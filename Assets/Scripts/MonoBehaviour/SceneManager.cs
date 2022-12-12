using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

	private void Start()
	{

	}

	private Dictionary<KlootnScene, string> klootnSceneNames = new Dictionary<KlootnScene, string>()
		{
			{ KlootnScene.MainMenu, "MainMenu" },
			{ KlootnScene.GameBallThrow, "BallThrowGame" },
		};

	public void LoadScene(KlootnScene klootnScene)
	{
		if (GetCurrentScene() != klootnScene)
			UnityEngine.SceneManagement.SceneManager.LoadScene(klootnSceneNames[klootnScene]);
		else Debug.Log($"Did not load scene \"{klootnScene}\": Scene is already active");
	}

	public KlootnScene? GetCurrentScene()
	{
		UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

		string currentSceneName = currentScene.name;

		return GetKlootnScene(currentSceneName);
	}

	private KlootnScene? GetKlootnScene(string sceneName)
	{
		foreach (KeyValuePair<KlootnScene, string> pair in klootnSceneNames)
		{
			if (pair.Value == sceneName) return pair.Key;
		}
		return null;
	}
}
