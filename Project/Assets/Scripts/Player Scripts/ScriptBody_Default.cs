﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScriptBody_Default : NetworkBehaviour
{
    private bool isLocalPlayerDerived;           //True if the parent player is a local player.
    public bool IsLocalPlayerDerived
    {
        set { isLocalPlayerDerived = value; }
    }

    public Camera cam;                          //Camera of body

    public int health;                          //Current health
    private int STARTING_HEALTH;                //Starting health

    //HUD elements
    private Text displayHealth;

    // Use this for initialization
    void Start()
    {
        //Health and health display
        STARTING_HEALTH = health;
        displayHealth = GameObject.Find("TextHealth").GetComponent<Text>();
        displayHealth.text = "Health: " + health;
    }

    public void CheckCamera()
    {
        if(!isLocalPlayerDerived)
        {
            cam.enabled = false;
        }
    }

    //Lose and display health
    public void LoseHealth(Component bulletScript)
    {
        if (isLocalPlayerDerived)
        {
            health--;
            print("lost health!");
            if (health <= 0)
            {
                //do something
                print("respawn");
                health = STARTING_HEALTH;
            }

            displayHealth.text = "Health: " + health;                //UI display
            NetworkServer.Destroy(bulletScript.gameObject);
        }
    }
}
