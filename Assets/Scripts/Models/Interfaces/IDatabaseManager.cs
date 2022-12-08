using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDatabaseManager
{
    public IEnumerator RegisterPlayer(NewPlayer newPlayer, Action<Player> onCallback);
}
