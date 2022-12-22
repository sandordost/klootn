
using System;
using System.Collections.Generic;

public class LobbiesChangedEventArgs : EventArgs
{
	public Dictionary<string, LobbyChangeState> ChangedLobbies { get; set; }
}