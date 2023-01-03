using Firebase.Firestore;
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
	public Task<Player> RegisterPlayer(Player newPlayer);

	/// <summary>
	/// Checks whether <paramref name="newPlayer"/> exists in the <b>Database</b>. 
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns>If <paramref name="newPlayer"/> exists : (new)<see cref="Player"/>. If it doesn't exist : null</returns>
	public Task<Player> Login(Player newPlayer);

	/// <summary>
	/// Checks whether <paramref name="newPlayer"/> exists. 
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns>The answer in <see cref="Boolean"/> format</returns>
	public Task<bool> PlayerExists(Player newPlayer);

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
	public Task<List<Player>> GetAllPlayers();

	/// <summary>
	/// Gets all players from the database found with <paramref name="playerIds"/>
	/// </summary>
	/// <param name="playerIds"></param>
	/// <returns>A new <see cref="List{T}"/> with the class <see cref="Player"/></returns>
	public Task<List<Player>> GetAllPlayers(string[] playerIds);

	/// <summary>
	/// Adds a new lobby to the database
	/// </summary>
	/// <param name="host"></param>
	/// <returns><see cref="Lobby"/> that was created in the database</returns>
	public Task<Lobby> CreateLobby(Player host, string name, string description, string mapId);

	/// <summary>
	/// Gets player by name from the database
	/// </summary>
	/// <param name="name"></param>
	/// <returns>New <see cref="Player"/>, is <b>null</b> when not found</returns>
	public Task<Player> GetPlayerByName(string name);

	/// <summary>
	/// Gets a player with id: <paramref name="id"/> from the database
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Task<Player> GetPlayerById(string id);

	/// <summary>
	/// Gets a Lobby by LobbyId from the database
	/// </summary>
	/// <param name="id"></param>
	/// <returns>New <see cref="Lobby"/>, returns <b>null</b> when not found</returns>
	public Task<Lobby> GetLobby(string id);

	/// <summary>
	/// Adds a player to the lobby in the database
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	public Task AddPlayerToLobby(string lobbyId, string playerId);

	/// <summary>
	/// Adds or updates last seen state for a player in a lobby.
	/// </summary>
	/// <param name="playerId"></param>
	/// <param name="timestamp"></param>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public Task UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp);

	/// <summary>
	/// Updates the last seen field for a player
	/// </summary>
	/// <param name="playerId"></param>
	/// <param name="timestamp"></param>
	/// <returns></returns>
	public Task UpdateLastSeen(string playerId, Timestamp timestamp);

	/// <summary>
	/// Removes players when they have been inactive for <paramref name="secondsTimeout"/> time
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public Task RemoveInactivePlayersFromLobby(string lobbyId, int secondsTimeout);

	/// <summary>
	/// Removes a player from a lobby
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	/// <returns></returns>
	public Task RemovePlayerFromLobby(string lobbyId, string playerId);

	/// <summary>
	/// Gets all current players Ids from the lobby
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public Task<List<Player>> GetLobbyPlayers(string lobbyId);

	/// <summary>
	/// Removes entry of a player in the LastSeen list
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	/// <returns></returns>
	public Task RemoveLobbyPlayerData(string lobbyId, string playerId);

	/// <summary>
	/// Updates the lobby mapId, within a database
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="mapId"></param>
	/// <returns></returns>
	public Task UpdateLobbyMap(string lobbyId, string mapId);

	/// <summary>
	/// Gets the mapId from a lobby that is found using <paramref name="lobbyId"/>
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public Task<string> GetLobbyMap(string lobbyId);

	/// <summary>
	/// Gets the playerId that is host of the lobby found with <paramref name="lobbyId"/>
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public Task<string> GetLobbyHost(string lobbyId);

	/// <summary>
	/// Sets the host to <paramref name="playerId"/> of the lobby found with <paramref name="lobbyId"/> 
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	/// <returns></returns>
	public Task SetHost(string lobbyId, string playerId);

	/// <summary>
	/// Gets all lobbies and removes any lobby that does not have any players
	/// </summary>
	/// <returns></returns>
	public Task RemoveEmptyLobbies();

	/// <summary>
	/// Removes a lobby from lobbies in the database
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public Task RemoveLobby(string lobbyId);

	/// <summary>
	/// Gets a dictionary of player ids and the color of that player
	/// </summary>
	/// <returns></returns>
	public Task<Dictionary<string, PlayerColor>> GetLobbyColors(string lobbyId);

	/// <summary>
	/// Gets the current playercolor of the player with the id <paramref name="playerId"/>
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	/// <returns></returns>
	public Task<PlayerColor> GetPlayerColor(string lobbyId, string playerId);

	/// <summary>
	/// Sets the player color of player <paramref name="playerId"/> in the lobby with id <paramref name="lobbyId"/> in the database using <paramref name="color"/>
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	/// <param name="color"></param>
	/// <returns></returns>
	public Task UpdatePlayerColor(string lobbyId, string playerId, PlayerColor color);

}
