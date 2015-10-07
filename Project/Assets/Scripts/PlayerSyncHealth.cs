using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSyncHealth : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerHealthSynced")]
    private int health;     //Health

    private int aHealth; //Health

    //Displays health on each update. Pretty inefficient, could add some boolean states to block the repetition.
    void Update()
    {
        DisplayHealth();
    }

    //Changes the Opponent's health display to be the opponent's health.
    void DisplayHealth()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("TextHealthOpponent").GetComponent<Text>().text = "Opponent's Health: " + health;
        }
    }

    //Server set new health
    [Command]
    void CmdSendNewHealthToServer(int newHealth)
    {
        health = newHealth;
    }

    //Transmit new health to server
    [Client]
    public void TransmitHealth(int newHealth)
    {
        if (!isLocalPlayer && CheckIfNewHealth(aHealth, newHealth))
        {
            aHealth = newHealth;
            CmdSendNewHealthToServer(newHealth);
        }
    }

    //Check if new health is different from old
    bool CheckIfNewHealth(int health1, int health2)
    {
        if (health1 == health2)
        {
            return false;
        }
        return true;
    }

    //Sync healths
    [Client]
    void OnPlayerHealthSynced(int aHealth)
    {
        health = aHealth;
    }
}