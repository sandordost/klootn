using System;
using System.Collections;

public interface IDatabaseManager
{
	/// <summary>
	/// Tries to add the <paramref name="newPlayer"/> to the <b>Database</b>. 
	/// <para>
	/// If it succeeds <paramref name="callback"/> will be invoked with <see cref="Player"/> object. 
	/// If it does not succeed, <see cref="Player"/> will be null
	/// </para>
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator RegisterPlayer(NewPlayer newPlayer, Action<Player> callback);

	/// <summary>
	/// Checks whether <paramref name="newPlayer"/> exists in the <b>Database</b>. 
	/// <para>
	/// If it does; <paramref name="callback"/> will contain the object <see cref="Player"/>. 
	/// If it doesn't exist. <see cref="Player"/> will be null
	/// </para>
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator Login(NewPlayer newPlayer, Action<Player> callback);

	/// <summary>
	/// Checks whether <paramref name="newPlayer"/> exists. 
	/// <para>
	/// <paramref name="callback"/> with be invoked with the answer in <see cref="Boolean"/> format
	/// </para>
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator PlayerExists(NewPlayer newPlayer, Action<bool> callback);

	/// <summary>
	/// Gets the latest <see cref="Motd"/>. Invoking <paramref name="callback"/> with the latest <see cref="Motd"/>
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator GetLatestMotd(Action<Motd> callback);
}
