using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDatabaseManager
{
	/// <summary>
	/// Gets all existing <b>lobbies</b> in the database
	/// </summary>
	/// <returns><see cref="List{T}"/> of <see cref="Lobby"/> objects</returns>
	public Task<List<Lobby>> GetLobbies();

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

	/// <summary>
	/// Gets all players from the database
	/// </summary>
	/// <returns>A new <see cref="List{T}"/> with the class <see cref="Player"/></returns>
	public Task<List<Player>> GetPlayers();

	/// <summary>
	/// Adds a new lobby to the database
	/// </summary>
	/// <param name="host"></param>
	/// <returns><see cref="Lobby"/> that was created in the database</returns>
	public Task<Lobby> CreateLobby(Player host, string title, string description);

	public Task<Player> GetPlayerByName(string name);
}
