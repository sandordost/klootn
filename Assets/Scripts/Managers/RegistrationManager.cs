using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegistrationManager : MonoBehaviour
{
	private Player player;
	DatabaseReference databaseReference;
	public TMP_InputField usernameInputField;
	public TMP_InputField passwordInputField;

	private void Start()
	{
		databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	public void RegisterPlayer()
	{
		player = new Player(usernameInputField.text, passwordInputField.text);
		string json = JsonUtility.ToJson(player);

		databaseReference.Child("Players").Child(SystemInfo.deviceUniqueIdentifier).SetRawJsonValueAsync(json);
		Debug.Log(json);
	}
}
