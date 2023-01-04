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
	public IEnumerator GetLobbies(Action<List<Lobby>> callback);

	/// <summary>
	/// Tries to add the <paramref name="newPlayer"/> to the <b>Database</b>. 
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns>If registration succeeds : (new)<see cref="Player"/>. If it doesn't : null</returns>
	public IEnumerator RegisterPlayer(Player newPlayer, Action<Player> callback);

	/// <summary>
	/// Checks whether <paramref name="newPlayer"/> exists in the <b>Database</b>. 
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns>If <paramref name="newPlayer"/> exists : (new)<see cref="Player"/>. If it doesn't exist : null</returns>
	public IEnumerator Login(Player newPlayer, Action<Player> callback);

	/// <summary>
	/// Checks whether <paramref name="newPlayer"/> exists. 
	/// </summary>
	/// <param name="newPlayer"></param>
	/// <param name="callback"></param>
	/// <returns>The answer in <see cref="Boolean"/> format</returns>
	public IEnumerator PlayerExists(Player newPlayer, Action<bool> callback);

	/// <summary>
	/// Gets the latest <see cref="Motd"/> and returns it.
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator GetLatestMotd(Action<Motd> callback);

	/// <summary>
	/// Gets all players from the database
	/// </summary>
	/// <returns>A new <see cref="List{T}"/> with the class <see cref="Player"/></returns>
	public IEnumerator GetAllPlayers(Action<List<Player>> callback);

	/// <summary>
	/// Gets all players from the database found with <paramref name="playerIds"/>
	/// </summary>
	/// <param name="playerIds"></param>
	/// <returns>A new <see cref="List{T}"/> with the class <see cref="Player"/></returns>
	public IEnumerator GetAllPlayers(string[] playerIds, Action<List<Player>> callback);

	/// <summary>
	/// Adds a new lobby to the database
	/// </summary>
	/// <param name="host"></param>
	/// <returns><see cref="Lobby"/> that was created in the database</returns>
	public IEnumerator CreateLobby(Player host, string name, string description, string mapId, Action<Lobby> callback);

	/// <summary>
	/// Gets player by name from the database
	/// </summary>
	/// <param name="name"></param>
	/// <returns>New <see cref="Player"/>, is <b>null</b> when not found</returns>
	public IEnumerator GetPlayerByName(string name, Action<Player> callback);

	/// <summary>
	/// Gets a player with id: <paramref name="id"/> from the database
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public IEnumerator GetPlayerById(string id, Action<Player> callback);

	/// <summary>
	/// Gets a Lobby by LobbyId from the database
	/// </summary>
	/// <param name="id"></param>
	/// <returns>New <see cref="Lobby"/>, returns <b>null</b> when not found</returns>
	public IEnumerator GetLobby(string id, Action<Lobby> callback);

	/// <summary>
	/// Adds a player to the lobby in the database
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	public IEnumerator AddPlayerToLobby(string lobbyId, string playerId);

	/// <summary>
	/// Adds or updates last seen state for a player in a lobby.
	/// </summary>
	/// <param name="playerId"></param>
	/// <param name="timestamp"></param>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public IEnumerator UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp);

	/// <summary>
	/// Updates the last seen field for a player
	/// </summary>
	/// <param name="playerId"></param>
	/// <param name="timestamp"></param>
	/// <returns></returns>
	public IEnumerator UpdateLastSeen(string playerId, Timestamp timestamp);

	/// <summary>
	/// Removes players when they have been inactive for <paramref name="secondsTimeout"/> time
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public IEnumerator RemoveInactivePlayersFromLobby(string lobbyId, int secondsTimeout);

	/// <summary>
	/// Removes a player from a lobby
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	/// <returns></returns>
	public IEnumerator RemovePlayerFromLobby(string lobbyId, string playerId);

	/// <summary>
	/// Gets all current players Ids from the lobby
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public IEnumerator GetLobbyPlayers(string lobbyId, Action<List<Player>> callback);

	/// <summary>
	/// Removes entry of a player in the LastSeen list
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	/// <returns></returns>
	public IEnumerator RemoveLobbyPlayerData(string lobbyId, string playerId);

	/// <summary>
	/// Updates the lobby mapId, within a database
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="mapId"></param>
	/// <returns></returns>
	public IEnumerator UpdateLobbyMap(string lobbyId, string mapId);

	/// <summary>
	/// Gets the mapId from a lobby that is found using <paramref name="lobbyId"/>
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public IEnumerator GetLobbyMap(string lobbyId, Action<string> callback);

	/// <summary>
	/// Gets the playerId that is host of the lobby found with <paramref name="lobbyId"/>
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public IEnumerator GetLobbyHost(string lobbyId, Action<string> callback);

	/// <summary>
	/// Sets the host to <paramref name="playerId"/> of the lobby found with <paramref name="lobbyId"/> 
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	/// <returns></returns>
	public IEnumerator SetHost(string lobbyId, string playerId);

	/// <summary>
	/// Gets all lobbies and removes any lobby that does not have any players
	/// </summary>
	/// <returns></returns>
	public IEnumerator RemoveEmptyLobbies();

	/// <summary>
	/// Removes a lobby from lobbies in the database
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <returns></returns>
	public IEnumerator RemoveLobby(string lobbyId);

	/// <summary>
	/// Gets a dictionary of player ids and the color of that player
	/// </summary>
	/// <returns></returns>
	public IEnumerator GetLobbyColors(string lobbyId, Action<Dictionary<string, PlayerColor>> callback);

	/// <summary>
	/// Gets the current playercolor of the player with the id <paramref name="playerId"/>
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	/// <returns></returns>
	public IEnumerator GetPlayerColor(string lobbyId, string playerId, Action<PlayerColor> callback);

	/// <summary>
	/// Sets the player color of player <paramref name="playerId"/> in the lobby with id <paramref name="lobbyId"/> in the database using <paramref name="color"/>
	/// </summary>
	/// <param name="lobbyId"></param>
	/// <param name="playerId"></param>
	/// <param name="color"></param>
	/// <returns></returns>
	public IEnumerator UpdatePlayerColor(string lobbyId, string playerId, PlayerColor color);

}
