using UnityEngine;
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
	protected bool isShielded = false;

    // Use this for initialization
    void Start()
    {
        //set parentCore to Player's ScriptCore
		parentCore = this.gameObject.transform.parent.GetComponent<ScriptCore>();
		parentCore.maxHealth = STARTING_HEALTH;

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
	
	 //Lose and display health, return if died or not
    public bool LoseHealth(Component bulletScript, float damage)
    {
		bool died = false;
        if (isLocalPlayerDerived && !isShielded)
        {
            health -= damage;
            NetworkServer.Destroy(bulletScript.gameObject);
            if (health <= 0)
            {
                //do something
                health = STARTING_HEALTH;
				died = true;
            }
			
			parentCore.TransmitHealth(health);
        }
		return died;
    }
	protected void GainHealth(float healing)
    {
        if (isLocalPlayerDerived)
        {
            health += healing;
			parentCore.TransmitHealth(health);
        }
    }
}
