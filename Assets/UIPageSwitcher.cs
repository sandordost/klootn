using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPageSwitcher : MonoBehaviour
{
    public GameObject[] pages; 

    public void SwitchPage(GameObject page)
    {
        foreach(GameObject obj in pages)
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
