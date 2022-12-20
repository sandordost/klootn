using Firebase.Firestore;

[FirestoreData]
public class NewPlayer : Player
{
	[FirestoreProperty]
	public string Password { get; set; }

	public NewPlayer()
	{

	}

	public NewPlayer(string name, string id, string password) : base(name, id)
	{
		this.Password = password;
	}

	public NewPlayer(string name, string password) : this(name, "", password)
	{

	}
}