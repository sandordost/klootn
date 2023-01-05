using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;

[FirestoreData]
[Serializable]
public class Lobby
{
	//[FirestoreDocumentId]
	//public string Id { get; set; }
	public string Id;

	//[FirestoreProperty]
	//public List<string> Players { get; set; }
	public List<string> Players;

	//[FirestoreProperty]
	//public Dictionary<string, LobbyPlayerData> LobbyPlayersData { get; set; }
	public Dictionary<string, LobbyPlayerData> LobbyPlayersData;

	//[FirestoreProperty]
	//public string HostId { get; set; }
	public string HostId;

	//[FirestoreProperty]
	//public string Name { get; set; }
	public string Name;

	//[FirestoreProperty]
	//public string MapId { get; set; }
	public string MapId;

	//[FirestoreProperty]
	//public string Description { get; set; }
	public string Description;
}

public static class LobbyExtensions
{
	public static bool CompareLobby(this Lobby currentLobby, Lobby comparedLobby)
	{
		if (currentLobby.Id.Equals(comparedLobby.Id) &&
			currentLobby.Name.Equals(comparedLobby.Name) &&
			currentLobby.Description.Equals(comparedLobby.Description) &&
			currentLobby.Players.SequenceEqual(comparedLobby.Players) &&
			currentLobby.MapId.Equals(comparedLobby.MapId) &&
			currentLobby.HostId.Equals(comparedLobby.HostId))
		{
			return true;
		}
		return false;
	}
}
