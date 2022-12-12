using System;
using System.Collections;
using UnityEngine;

public interface IStorageManager
{
	/// <summary>
	/// Gets an (<see cref="Texture"/>)Image from the <b>Cloud Storage</b> using the <see cref="Motd.ImageUrl"/> and passes the <see cref="Texture"/> via <paramref name="callback"/>
	/// </summary>
	/// <param name="motd"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator GetImage(Motd motd, Action<Texture> callback);
}
