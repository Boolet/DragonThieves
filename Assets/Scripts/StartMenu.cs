using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

	[SerializeField] RectTransform rootMenu;
	[SerializeField] Text modeButtonText;
	[SerializeField] RectTransform joinMenu;
	[SerializeField] RectTransform hostMenu;

	string targetAddress;
	int targetPort;

	public void HostMenuButton()
    {
		//NetworkManager.singleton.ServerChangeScene();
    }

	public void ModeButton(){
		//toggles the additional gravity directions
		DominoSpawner.fixedGravityMode = !DominoSpawner.fixedGravityMode;

		modeButtonText.text = "Mode:Sandbox";
		if (DominoSpawner.fixedGravityMode == false)
			modeButtonText.text = "Mode:Gravity";
	}

	public void JoinMenuButton(){
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

	public void SetConnectionAddress(string address){
		targetAddress = address;
	}

	public void SetConnectionPort(int port){
		targetPort = port;
	}

	void TryHost(){
		NetworkManager.singleton.StartHost();
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
