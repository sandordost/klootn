using Firebase.Firestore;
using System;

[FirestoreData]
[Serializable]
public class Motd
{
	//[FirestoreDocumentId]
	//public string Id { get; set; }
	public string Id;

	//[FirestoreProperty]
	//public string Title { get; set; }
	public string Title;

	//[FirestoreProperty]
	//public string Message { get; set; }
	public string Message;

	//[FirestoreProperty]
	//public string ImageUrl { get; set; }
	public string ImageUrl;

	//[FirestoreProperty]
	//public Timestamp Uploaddate { get; set; }
	public string Uploaddate;
}