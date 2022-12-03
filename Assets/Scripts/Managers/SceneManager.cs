using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
	private Dictionary<KlootnScene, string> klootnSceneNames = new Dictionary<KlootnScene, string>()
		{
			{ KlootnScene.MainMenu, "MainMenu" },
			{ KlootnScene.GameBallThrow, "BallThrowGame" },
		};

	public void LoadScene(KlootnScene klootnScene)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(klootnSceneNames[klootnScene]);
	}
}

public enum KlootnScene
{
	MainMenu,
	GameBallThrow
}
