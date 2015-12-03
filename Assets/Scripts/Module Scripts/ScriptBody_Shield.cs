using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptBody_Shield : ScriptBody_Default
{
	private float cooldown = 20;
	private float timeLeft = 0;
	private float duration = 1.5f;
	private float shielding = 0;
	
	void Update()
	{
		if( isLocalPlayerDerived )
		{
			//link shield activation to Q
			if (Input.GetKey("Q") && timeLeft <= 0)
			{
				shielding = duration;
				timeLeft = cooldown;
			}
			
			if( shielding <= Time.deltaTime ) {
				//shield fade animation
			}
			
			shielding -= Time.deltaTime;
			timeLeft -= cooldown;
			
			if( timeLeft <= 0 )
			{
				//display ability can be actived
			}
		}
	}
	
	public override void LoseHealth(Component bulletScript, float damage)
    {
        if (isLocalPlayerDerived && shielding <= 0)
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
}
