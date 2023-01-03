
using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SQLDatabaseManager : IDatabaseManager
{
	public Task AddPlayerToLobby(string lobbyId, string playerId)
	{
		throw new System.NotImplementedException();
	}

	public Task<Lobby> CreateLobby(Player host, string name, string description, string mapId)
	{
		throw new System.NotImplementedException();
	}

	public Task<List<Player>> GetAllPlayers()
	{
		throw new System.NotImplementedException();
	}

	public Task<List<Player>> GetAllPlayers(string[] playerIds)
	{
		throw new System.NotImplementedException();
	}

	public Task<Motd> GetLatestMotd()
	{
		throw new System.NotImplementedException();
	}

	public Task<List<Lobby>> GetLobbies()
	{
		throw new System.NotImplementedException();
	}

	public Task<Lobby> GetLobby(string id)
	{
		throw new System.NotImplementedException();
	}

	public Task<Dictionary<string, PlayerColor>> GetLobbyColors(string lobbyId)
	{
		throw new System.NotImplementedException();
	}

	public Task<string> GetLobbyHost(string lobbyId)
	{
		throw new System.NotImplementedException();
	}

	public Task<string> GetLobbyMap(string lobbyId)
	{
		throw new System.NotImplementedException();
	}

	public Task<List<Player>> GetLobbyPlayers(string lobbyId)
	{
		throw new System.NotImplementedException();
	}

	public Task<Player> GetPlayerById(string id)
	{
		throw new System.NotImplementedException();
	}

	public Task<Player> GetPlayerByName(string name)
	{
		throw new System.NotImplementedException();
	}

	public Task<PlayerColor> GetPlayerColor(string lobbyId, string playerId)
	{
		throw new System.NotImplementedException();
	}

	public Task<Player> Login(Player newPlayer)
	{
		throw new System.NotImplementedException();
	}

	public Task<bool> PlayerExists(Player newPlayer)
	{
		throw new System.NotImplementedException();
	}

	public Task<Player> RegisterPlayer(Player newPlayer)
	{
		throw new System.NotImplementedException();
	}

	public Task RemoveEmptyLobbies()
	{
		throw new System.NotImplementedException();
	}

	public Task RemoveInactivePlayersFromLobby(string lobbyId, int secondsTimeout)
	{
		throw new System.NotImplementedException();
	}

	public Task RemoveLobby(string lobbyId)
	{
		throw new System.NotImplementedException();
	}

	public Task RemoveLobbyPlayerData(string lobbyId, string playerId)
	{
		throw new System.NotImplementedException();
	}

	public Task RemovePlayerFromLobby(string lobbyId, string playerId)
	{
		throw new System.NotImplementedException();
	}

	public Task SetHost(string lobbyId, string playerId)
	{
		throw new System.NotImplementedException();
	}

	public Task UpdateLastSeen(string playerId, Timestamp timestamp)
	{
		throw new System.NotImplementedException();
	}

	public Task UpdateLobbyLastSeen(string playerId, string lobbyId, Timestamp timestamp)
	{
		throw new System.NotImplementedException();
	}

	public Task UpdateLobbyMap(string lobbyId, string mapId)
	{
		throw new System.NotImplementedException();
	}

	public Task UpdatePlayerColor(string lobbyId, string playerId, PlayerColor color)
	{
		throw new System.NotImplementedException();
	}
}
