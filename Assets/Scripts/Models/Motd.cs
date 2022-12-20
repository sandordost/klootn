using Firebase.Firestore;
using System;

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

	[FirestoreProperty]
	public DateTime Uploaddate { get; set; }
}