using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetStartup : NetworkBehaviour
{
    public Camera cam;
    public Rigidbody rb;

	void Start ()
	{
		/*
		 * We turned off the character controller, firstperson controller, 
		 * the main camera and its audiolistener, because only OUR OWN character should respond to 
		 * keyboard events and control the camer, not all the other clients.
		 * 
		 * So here we turn them all back on IF IT'S US. 
		 * */
        
		if (isLocalPlayer)
        {
            cam.enabled = true;
            rb.isKinematic = false;
            GetComponent<PlayerScriptNN>().IsLocalPlayer = true;
        }
        else
        {
            GetComponent<PlayerScriptNN>().IsLocalPlayer = false;
        }
	}

}
