using Firebase.Firestore;

[FirestoreData]
public class Player
{
	[FirestoreDocumentId]
	public string Id { get; set; }

	[FirestoreProperty]
	public string Name { get; set; }

	public Player(string name, string id)
	{
		Name = name;
		Id = id;
	}

	public Player()
	{

	}
}
