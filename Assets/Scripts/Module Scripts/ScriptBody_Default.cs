﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScriptBody_Default : NetworkBehaviour
{
    protected bool isLocalPlayerDerived;           //True if the parent player is a local player.
    public bool IsLocalPlayerDerived
    {
        set { isLocalPlayerDerived = value; }
    }

    public Camera cam;                          //Camera of body
	
	public float health;
	protected float STARTING_HEALTH = 10;               //Starting health
	public float StartingHealth
	{
		get { return STARTING_HEALTH; }
	}

	protected ScriptCore parentCore;
    //HUD elements
    protected Text displayHealth;

    // Use this for initialization
    void Start()
    {
        //set parentCore to Player's ScriptCore
		parentCore = this.gameObject.transform.parent.GetComponent<ScriptCore>();

		if (isLocalPlayerDerived)
		{
			MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
			render.enabled = false;
		}

    }

    public void CheckCamera()
    {
        if(!isLocalPlayerDerived)
        {
            cam.enabled = false;
        }
    }
	
	 //Lose and display health
    public void LoseHealth(Component bulletScript, float damage)
    {
        if (isLocalPlayerDerived)
        {
            health -= damage;
            print("lost health!");
            if (health <= 0)
            {
                //do something
                parentCore.Spawn();
                health = STARTING_HEALTH;
            }
            NetworkServer.Destroy(bulletScript.gameObject);
			parentCore.TransmitHealth(health);
        }
    }
	protected void GainHealth(float healing)
    {
        if (isLocalPlayerDerived)
        {
            health += healing;
            print("gained health!");
			parentCore.TransmitHealth(health);
        }
    }
}
