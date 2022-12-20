using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;

[FirestoreData]
public class Lobby
{
	[FirestoreDocumentId]
	public string Id { get; set; }

	/// Dictionary of ids : string and players : Players
	[FirestoreProperty]
	public Dictionary<string, Player> Players { get; set; }

	[FirestoreProperty]
	public string HostId { get; set; }

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
			currentLobby.Description.Equals(comparedLobby.Description) &&
			currentLobby.Players.Keys.SequenceEqual(comparedLobby.Players.Keys))
		{
			return true;
		}
		return false;
	}
}
