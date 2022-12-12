using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
	/// <summary>
	/// Dictionary of <see cref="KlootnScene"/> and <see cref="string"/> sceneName
	/// </summary>
	public Dictionary<KlootnScene, string> klootnSceneNames = new Dictionary<KlootnScene, string>()
		{
			{ KlootnScene.MainMenu, "MainMenu" },
			{ KlootnScene.GameBallThrow, "BallThrowGame" },
		};

	/// <summary>
	/// Loads a scene using <paramref name="klootnScene"/>
	/// </summary>
	/// <param name="klootnScene"></param>
	public void LoadScene(KlootnScene klootnScene)
	{
		if (GetCurrentScene() != klootnScene)
			UnityEngine.SceneManagement.SceneManager.LoadScene(klootnSceneNames[klootnScene]);
		else Debug.Log($"Did not load scene \"{klootnScene}\": Scene is already active");
	}

	/// <summary>
	/// Gets the current <see cref="KlootnScene"/> depending on (<see cref="string"/>) <see cref="UnityEngine.SceneManagement.SceneManager.GetActiveScene()"/>.name
	/// </summary>
	/// <returns></returns>
	public KlootnScene? GetCurrentScene()
	{
		UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

		string currentSceneName = currentScene.name;

		return GetKlootnScene(currentSceneName);
	}

	/// <summary>
	/// Searches the <see cref="klootnSceneNames"/> for the <see cref="string"/> <paramref name="sceneName"/> and returns the (<see cref="KlootnScene"/>)key
	/// </summary>
	/// <param name="sceneName"></param>
	/// <returns></returns>
	private KlootnScene? GetKlootnScene(string sceneName)
	{
		foreach (KeyValuePair<KlootnScene, string> pair in klootnSceneNames)
		{
			if (pair.Value == sceneName) return pair.Key;
		}
		return null;
	}
}
