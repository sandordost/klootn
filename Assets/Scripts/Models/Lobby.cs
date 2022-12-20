using System.Collections.Generic;
using System.Linq;
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

public static class LobbyExtensions
{
	public static bool CompareLobby(this Lobby currentLobby, Lobby comparedLobby)
	{
		if (currentLobby.Id.Equals(comparedLobby.Id) &&
			currentLobby.Name.Equals(comparedLobby.Name) &&
			currentLobby.PlayerIds.SequenceEqual(comparedLobby.PlayerIds))
		{
			return true;
		}
		return false;
	}
}
