using UnityEngine;
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
    public int aHealth;                          //Current health
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
    public void LoseHealth(Component bulletScript, int damage)
    {
        if (isLocalPlayerDerived)
        {
            health -= damage;
            print("lost health!");
            if (health <= 0)
            {
                //do something
                this.transform.parent.gameObject.GetComponent<ScriptCore>().Spawn();
                health = STARTING_HEALTH;
            }
            displayHealth.text = "Health: " + health;                //UI display
            NetworkServer.Destroy(bulletScript.gameObject);
        }
        else
        {
            health--;
            print("lost health!");
            if (health <= 0)
            {
                //do something
                this.transform.parent.gameObject.GetComponent<ScriptCore>().Spawn();
                health = STARTING_HEALTH;
            }
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
