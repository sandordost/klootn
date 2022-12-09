using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

[FirestoreData]
public class Player
{
	[FirestoreDocumentId]
	public string id { get; set; }

	[FirestoreProperty]
	public string name { get; set; }

	public Player()
	{

	}

	public Player(string name, string id)
	{
		this.name = name;
		this.id = id;
	}
}
