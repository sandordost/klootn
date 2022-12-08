using Firebase.Firestore;

[FirestoreData]
public class NewPlayer : Player
{
	[FirestoreProperty]
	public string password { get; set; }

	public NewPlayer()
	{

	}

	public NewPlayer(string name, string id, string password) : base(name, id)
	{
		this.password = password;
	}
}