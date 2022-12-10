using Firebase.Firestore;

[FirestoreData]
public class Motd
{
	[FirestoreDocumentId]
	public string Id { get; set; }

	[FirestoreProperty]
	public string Title { get; set; }

	[FirestoreProperty]
	public string Message { get; set; }

	[FirestoreProperty]
	public string ImageUrl { get; set; }
}