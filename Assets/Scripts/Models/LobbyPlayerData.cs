using Firebase.Firestore;

[FirestoreData]
public class LobbyPlayerData
{
	[FirestoreProperty]
	public Timestamp LastSeen { get; set; }

	[FirestoreProperty]
	public PlayerColor Color { get; set; }
}