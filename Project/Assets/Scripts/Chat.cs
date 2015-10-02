using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Chat : NetworkBehaviour
{
	const short chatMsg = MsgType.Highest + 1;

	private SyncListString chatLog = new SyncListString ();

	public static NetworkClient myClient;

	[SerializeField]
	InputField
		chatInput;
	[SerializeField]
	Text
		chatWindow;
	


//	 Use this for initialization
	void Start ()
	{

		chatLog.Callback = OnChatUpdated;

		//setup text boxes
		chatWindow.text = "";

		NetworkServer.RegisterHandler (chatMsg, OnServerPostChatMessage);   

		chatInput.onEndEdit.AddListener (delegate {
			PostChatMessage (chatInput.text);
		}); 
	}

	public override void OnStartClient ()
	{
		//Callback is the delegate type used for SyncListChanged.
		chatLog.Callback = OnChatUpdated;
	}

	/*
	 * [Server] 
	 * A Custom Attribute that can be added to member functions of NetworkBehaviour scripts, 
	 * to make them only run on servers.
	 * 
	 * A [Server] method returns immediately if NetworkServer.active is not true, 
	 * and generates a warning on the console. This attribute can be put on member 
	 * functions that are meant to be only called on server. This would redundant for 
	 * [Command] functions, as being server-only is already enforced for them.
	 */
	[Server]
	void OnServerPostChatMessage (NetworkMessage netMsg)
	{
		string message = netMsg.ReadMessage<StringMessage> ().value;
		chatLog.Add (message);
	}
	/*
	 * [Client]
	 * makes a NetworkBehaviour script only run on clients.
	 * 
	 * A [Client] method returns immediately if NetworkClient.active is not true, 
	 * and generates a warning on the console. This attribute can be put on member 
	 * functions that are meant to be only called on clients. This would redundant 
	 * for [ClientRPC] functions, as being client-only is already enforced for them.
	 */
	[Client]
	public void PostChatMessage (string message)
	{
		if (message.Length == 0)
			return;
		var msg = new StringMessage (message);
		NetworkManager.singleton.client.Send (chatMsg, msg);
		
		chatInput.text = "";
		chatInput.Select ();
		chatInput.ActivateInputField ();
		//scroll to bottom
		GameObject win = GameObject.Find ("Content");
		RectTransform rt = (RectTransform)win.transform;
		float y = rt.position.y + rt.sizeDelta.y + 2;
		rt.position = new Vector3 (rt.position.x, y, rt.position.y);
	}

	//callback we registered for when the syncList changes
	private void OnChatUpdated (SyncListString.Operation op, int index)
	{
		chatWindow.text += chatLog [chatLog.Count - 1] + "\n";
	}



}
