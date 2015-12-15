using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NetworkManagerCustom: NetworkManager
{
    //get input from text input fields
    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("txtIP").transform.FindChild("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = "127.0.0.1";  //ipAddress;
        if(ipAddress != "")
            NetworkManager.singleton.networkAddress = ipAddress;

        print(NetworkManager.singleton.networkAddress);
    }

    void SetPort ()
	{
		NetworkManager.singleton.networkPort = 11446;
	}
	
	//Button event to start hosting a game.
	public void StartupHost ()
	{
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartHost ();
	}
	
    //Button event to join a game.
	public void JoinGame ()
	{
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartClient ();
	}

	//Button event to navigate to the instructions screen
	public void LoadMenu (int level)
	{
		Application.LoadLevel (level);
	}
	
    //Disconnect from a game
	public void Disconnect ()
	{
		NetworkManager.singleton.StopHost ();
	}
	
	//When a level loads, setup cooresponding UI buttons
	void OnLevelWasLoaded (int level)
	{
		if (level == 0)
        {
			SetupLoginButtons ();
		}
        else 
        {
			SetupChatSceneButtons ();
		}
	}
	
	//Set up login buttons.
	void SetupLoginButtons ()
	{
		GameObject.Find ("btnHostGame").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnHostGame").GetComponent<Button> ().onClick.AddListener (StartupHost);
		
		GameObject.Find ("btnJoinGame").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnJoinGame").GetComponent<Button> ().onClick.AddListener (JoinGame);

	}
	
    //Set up chat.
	void SetupChatSceneButtons ()
	{
		GameObject.Find ("btnDisconnect").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnDisconnect").GetComponent<Button> ().onClick.AddListener (Disconnect);
	}
}