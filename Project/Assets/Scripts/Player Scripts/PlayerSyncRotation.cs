using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerSyncRotation: NetworkBehaviour
{
	
	[SyncVar (hook = "OnPlayerRotSynced")]
    private Vector3 syncPlayerRotation;
	
	[SerializeField]
	private float lerpRate = 20;
	
    private Vector3 lastPlayerRot;
	private float rotationThreshold = 0.2f;
	
    //Updates every frame.
	void Update ()
	{
        LerpRotations();       //Lerp rotations every frame.
	}
	
    //Updates at constant rate.
	void FixedUpdate ()
	{
		TransmitRotations ();   //TransmitRotations every fixed update.
	}
	
    //Lerp rotations for other-players.
	void LerpRotations ()
	{
		if (!isLocalPlayer)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(syncPlayerRotation), lerpRate * Time.deltaTime);
		}
	}
	
    //Server set rotations
	[Command]
	void CmdSendRotationsToServer (Vector3 playerRot)
	{
		//runs on server, we call on client
		syncPlayerRotation = playerRot;
	}
	
    //Transmit rotations to server.
	[Client]
	void TransmitRotations ()
	{
        //If local player is beyond rotation threshold.
		if (isLocalPlayer && CheckIfBeyondThreshold (transform.localEulerAngles, lastPlayerRot))
		{
			lastPlayerRot = transform.localEulerAngles;
			CmdSendRotationsToServer(lastPlayerRot);
		}
	}
	
    //Check if any rotation differences are beyond the threshold
	bool CheckIfBeyondThreshold (Vector3 rot1, Vector3 rot2)
	{
		if (Mathf.Abs(rot1.x - rot2.x) > rotationThreshold ||
            Mathf.Abs(rot1.y - rot2.y) > rotationThreshold ||
            Mathf.Abs(rot1.z - rot2.z) > rotationThreshold)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
    //Sync rotations
	[Client]
	void OnPlayerRotSynced (Vector3 latestPlayerRot)
	{
		syncPlayerRotation = latestPlayerRot;
	}
}
