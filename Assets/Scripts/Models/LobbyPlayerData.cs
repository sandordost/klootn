using Firebase.Firestore;
using System;

[FirestoreData]
[Serializable]
public class LobbyPlayerData
{
	public string Id;

	public string LobbyId;

	public string PlayerId;

	//[FirestoreProperty]
	//public Timestamp LastSeen { get; set; }
	public DateTime LastSeen;

	//[FirestoreProperty]
	//public PlayerColor Color { get; set; }
	public PlayerColor Color;
}