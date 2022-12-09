using System;
using System.Collections;
using UnityEngine;

public interface IStorageManager
{
	public IEnumerator GetImage(Motd motd, Action<Texture> callback);
}
