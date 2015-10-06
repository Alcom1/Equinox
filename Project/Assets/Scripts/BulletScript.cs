using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class BulletScript : NetworkBehaviour
{
	[SerializeField]
	private bool useHistoricalLerping = true;

	[SyncVar (hook = "SyncPositionValues")]
	private Vector3 syncPos;

	private float lerpRate;
	private float normalLerpRate = 16;
	private float fasterLerpRate = 27;
    private float lifetime = 30; // this is a total guess

	//variables to only send data when it's changed beyond a threshold.
	private Vector3 lastPosition;
	private float positionThreshold = 0.1f;
	private List<Vector3> syncPosList = new List<Vector3> ();
	
	void Start ()
	{
		lerpRate = normalLerpRate;
	}

    //Updates every frame.
	void Update ()
	{
		LerpPosition ();
        lifetime -= lerpRate/10;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
	}

    //Kills bullet on hit, hurts player(s) if they were hit
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerScriptNN>().LoseHealth();
        }
        Destroy(gameObject);
    }

    //Updates at constant rate.
    void FixedUpdate ()
	{
		TransmitPosition ();
    }

    //Lerp
    void LerpPosition()
    {
        //only on non-client characters, not us
        //smootly move from our old position data to our updated data we got from the server.
        if (!isLocalPlayer)
        {
            if (useHistoricalLerping)
            {
                HistoricalLerp();
            }
            else
            {
                OrdinaryLerp();
            }
        }
    }

    void OrdinaryLerp()
    {
        transform.position = Vector3.Lerp(transform.position, syncPos, Time.deltaTime * lerpRate);
    }

    void HistoricalLerp()
    {
        if (syncPosList.Count > 0)
        {
            //Set position
            transform.position = Vector3.Lerp(transform.position, syncPosList[0], Time.deltaTime * lerpRate);

            //if we're getting really close to that point, delete it
            if (Vector3.Distance(transform.position, syncPosList[0]) < positionThreshold)
            {
                syncPosList.RemoveAt(0);
            }

            // if we don't have so many in the list, lerp faster
            if (syncPosList.Count > 10)
            {
                lerpRate = fasterLerpRate;
            }
            else
            {
                lerpRate = normalLerpRate;
            }
        }
    }

    //Server set position
	[Command]
	void CmdSendPositionToServer (Vector3 pos)
	{
		//runs on server, we call on client
		syncPos = pos;
	}
	
	[Client]
	void TransmitPosition ()
	{
		// Send a command to the server to update bullet position, and 
		// it will update a SyncVar, which then automagically updates on everyone's game instance
		CmdSendPositionToServer (transform.position);
		lastPosition = transform.position;
	}

	//Sync position
	[Client]
	void SyncPositionValues (Vector3 latestPos)
	{
		syncPos = latestPos;
		syncPosList.Add (syncPos);
	}
}