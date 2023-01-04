using Firebase.Firestore;
using System;

[FirestoreData]
[Serializable]
public class Player
{
	//[FirestoreDocumentId]
	//public string Id { get; set; }
	public string Id;

	//[FirestoreProperty]
	//public string Name { get; set; }
	public string Name;

	//[FirestoreProperty]
	//public string Password { get; set; }
	public string Password;

	//[FirestoreProperty]
	//public Timestamp LastSeen { get; set; }
	public string LastSeen;

	public Player(string name, string id)
	{
		Name = name;
		Id = id;
	}

	public Player(string name, KlootnPassword password)
	{
		Name = name;
		Password = password.Password;
	}

	public Player()
	{

	}
}

public static class PlayerExtension
{
	public static bool ComparePlayer(this Player player, Player playerToCompare)
	{
		if (player.Id.Equals(playerToCompare.Id)
			&& player.Name.Equals(playerToCompare.Name))
			return true;

		return false;
	}
}
