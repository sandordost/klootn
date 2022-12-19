using System.Collections.Generic;
using Firebase.Firestore;

public class Lobby
{
	[FirestoreDocumentId]
	public string Id { get; set; }

	[FirestoreProperty]
	public List<Player> Players { get; set; }

	[FirestoreProperty]
	public Player Host { get; set; }

	[FirestoreProperty]
	public string Name { get; set; }

	[FirestoreProperty]
	public string Description { get; set; }
}
