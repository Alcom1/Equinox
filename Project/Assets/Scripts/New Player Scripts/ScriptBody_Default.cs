using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScriptBody_Default : NetworkBehaviour
{
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

    //Lose and display health
    public void LoseHealth(Component bulletScript)
    {
        if (isLocalPlayer)
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
            Destroy(bulletScript.gameObject);
        }
    }
}
