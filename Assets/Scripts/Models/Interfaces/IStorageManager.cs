using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public interface IStorageManager
{
	/// <summary>
	/// Gets an (<see cref="Texture"/>)Image from the <b>Cloud Storage</b> using the <see cref="Motd.ImageUrl"/> and passes the <see cref="Texture"/>
	/// </summary>
	/// <param name="motd"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public Task<Texture> GetImage(Motd motd);
}
