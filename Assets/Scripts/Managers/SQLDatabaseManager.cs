using System;
using System.Collections;

public class SQLDatabaseManager : IDatabaseManager
{
	public IEnumerator GetLatestMotd(Action<Motd> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator Login(NewPlayer newPlayer, Action<Player> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator PlayerExists(NewPlayer newPlayer, Action<bool> callback)
	{
		throw new NotImplementedException();
	}

	public IEnumerator RegisterPlayer(NewPlayer newPlayer, Action<Player> onCallback)
	{
		throw new NotImplementedException();
	}
}
