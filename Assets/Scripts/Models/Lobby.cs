using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class Lobby
{
	[FirestoreDocumentId]
	public string Id { get; set; }

	[FirestoreProperty]
	public List<string> PlayerIds { get; set; }

	public List<Player> Players { get; set; }

	[FirestoreProperty]
	public Player Host { get; set; }

	[FirestoreProperty]
	public string Name { get; set; }

	[FirestoreProperty]
	public string Description { get; set; }
}
