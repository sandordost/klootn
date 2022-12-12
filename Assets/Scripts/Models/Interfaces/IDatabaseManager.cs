using System;
using System.Collections;
using System.Threading.Tasks;

public interface IDatabaseManager
{
	/// <summary>
	/// Tries to add the <paramref name="newPlayer"/> to the <b>Database</b>. 
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns>If registration succeeds : (new)<see cref="Player"/>. If it doesn't : null</returns>
	public Task<Player> RegisterPlayer(NewPlayer newPlayer);

	/// <summary>
	/// Checks whether <paramref name="newPlayer"/> exists in the <b>Database</b>. 
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns>If <paramref name="newPlayer"/> exists : (new)<see cref="Player"/>. If it doesn't exist : null</returns>
	public Task<Player> Login(NewPlayer newPlayer);

	/// <summary>
	/// Checks whether <paramref name="newPlayer"/> exists. 
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns>The answer in <see cref="Boolean"/> format</returns>
	public Task<bool> PlayerExists(NewPlayer newPlayer);

	/// <summary>
	/// Gets the latest <see cref="Motd"/> and returns it.
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	public Task<Motd> GetLatestMotd();
}
