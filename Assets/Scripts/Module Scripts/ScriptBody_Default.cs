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
	
	private float health;
    private float STARTING_HEALTH;                //Starting health
	public float StartingHealth
	{
		get { return STARTING_HEALTH; }
	}

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
}
