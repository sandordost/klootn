
using System;
using System.Collections.Generic;

public class LobbiesChangedEventArgs : EventArgs
{
	public Dictionary<Lobby, LobbyChangeState> ChangedLobbies { get; set; }
}