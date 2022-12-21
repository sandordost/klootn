using Firebase.Firestore;
using System;

[FirestoreData]
public class Player
{
	[FirestoreDocumentId]
	public string Id { get; set; }

	[FirestoreProperty]
	public string Name { get; set; }

	[FirestoreProperty]
	public string Password { get; set; }

	[FirestoreProperty]
	public DateTime LastSeen { get; set; }

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
