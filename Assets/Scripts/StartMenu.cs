using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

	[SerializeField] RectTransform rootMenu;
	[SerializeField] RectTransform joinMenu;
	[SerializeField] RectTransform hostMenu;

	string targetAddress;
	int targetPort;

	public void HostButton()
    {
		//NetworkManager.singleton.ServerChangeScene();
    }

	public void JoinButton(){
		HideAll();
		joinMenu.gameObject.SetActive(true);
	}

	public void ReturnToRoot(){
		HideAll();
		rootMenu.gameObject.SetActive(true);
	}

	public void ExitButton()
	{
		Application.Quit();
	}

	public void SetConnectionAddress(){

	}

	void TryConnect(){
		NetworkManager.singleton.networkAddress = targetAddress;
		NetworkManager.singleton.networkPort = targetPort;
		NetworkManager.singleton.StartClient();
	}

	void HideAll(){
		rootMenu.gameObject.SetActive(false);
		joinMenu.gameObject.SetActive(false);
		hostMenu.gameObject.SetActive(false);
	}
}
