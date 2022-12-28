using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
	public GameObject alertScreen;

	public GameObject messageView;

	public GameObject loadingView;

	public void ShowMessageAlert(string title, string description, string buttonText = "Okay")
	{
		messageView.transform.Find("Title").GetComponent<TMP_Text>().text = title;
		messageView.transform.Find("Description").GetComponent<TMP_Text>().text = description;
		messageView.transform.Find("OkButton").Find("ButtonText").GetComponent<TMP_Text>().text = buttonText;
		messageView.SetActive(true);
		ToggleAlertScreen(true);
	}

	public void ShowLoadingAlert(string loadingString)
	{
		loadingView.transform.Find("LoadingText").GetComponent<TMP_Text>().text = loadingString;
		loadingView.SetActive(true);
		ToggleAlertScreen(true);
	}

	public void CloseLoadingAlert()
	{
		loadingView.SetActive(false);
		ToggleAlertScreen(messageView.activeSelf || loadingView.activeSelf);
	}

	public void CloseMessageView()
	{
		messageView.SetActive(false);
		ToggleAlertScreen(messageView.activeSelf || loadingView.activeSelf);
	}

	private void ToggleAlertScreen(bool value)
	{
		alertScreen.SetActive(value);
	}
}
