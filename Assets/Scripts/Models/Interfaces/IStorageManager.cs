using System;
using System.Collections;
using UnityEngine;

public interface IStorageManager
{
	public IEnumerator GetMotdImage(Action<Texture> callback);
}
