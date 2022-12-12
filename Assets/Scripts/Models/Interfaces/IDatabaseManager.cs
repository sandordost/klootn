using System;
using System.Collections;

public interface IDatabaseManager
{
	public IEnumerator RegisterPlayer(NewPlayer newPlayer, Action<Player> callback);

	public IEnumerator Login(NewPlayer newPlayer, Action<Player> callback);

	public IEnumerator PlayerExists(NewPlayer newPlayer, Action<bool> callback);

	public IEnumerator GetLatestMotd(Action<Motd> callback);
}
