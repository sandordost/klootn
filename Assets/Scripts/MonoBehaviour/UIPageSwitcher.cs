using UnityEngine;

public class UIPageSwitcher : MonoBehaviour
{
	public GameObject[] pages;

	/// <summary>
	/// Sets the <paramref name="page"/> <see cref="GameObject"/> to active and disables all other <see cref="GameObject"/>'s in <see cref="pages"/>
	/// </summary>
	/// <param name="page"></param>
	public void SwitchPage(GameObject page)
	{
		foreach (GameObject obj in pages)
		{
			if (obj.Equals(page))
			{
				obj.SetActive(true);
			}
			else
			{
				obj.SetActive(false);
			}
		}
	}

	public void SwitchPage(string pageName)
	{
		SwitchPage(GetPage(pageName));
	}

	private GameObject GetPage(string pageName)
	{
		foreach(GameObject obj in pages)
		{
			if (obj.name.Equals(pageName))
				return obj;
		}
		return null;
	}
}
