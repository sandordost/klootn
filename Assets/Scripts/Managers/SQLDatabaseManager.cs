using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQLDatabaseManager : IDatabaseManager
{
	public IEnumerator Login(NewPlayer newPlayer, Action<Player> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator RegisterPlayer(NewPlayer newPlayer, Action<Player> onCallback)
	{
		throw new NotImplementedException();
	}
}
