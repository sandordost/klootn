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
}
