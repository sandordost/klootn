using System;
using System.Collections;
using System.Threading.Tasks;

public class SQLDatabaseManager : IDatabaseManager
{
	public Task<Motd> GetLatestMotd()
	{
		throw new NotImplementedException();
	}

	public Task<Player> Login(NewPlayer newPlayer)
	{
		throw new NotImplementedException();
	}

	public Task<bool> PlayerExists(NewPlayer newPlayer)
	{
		throw new NotImplementedException();
	}

	public Task<Player> RegisterPlayer(NewPlayer newPlayer)
	{
		throw new NotImplementedException();
	}
}
