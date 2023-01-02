using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Map", menuName = "Map")]
public class KlootnMap : ScriptableObject
{
	public string id;
	public string title;
	public string description;
	public int maxPlayers;
	public Sprite image;
	public string sceneName;
}