using System;
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
	public List<string> Players { get; set; }

	[FirestoreProperty]
	public Dictionary<string, Timestamp> PlayersLastSeen { get; set; }

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
			currentLobby.Players.SequenceEqual(comparedLobby.Players))
		{
			return true;
		}
		return false;
	}
}
