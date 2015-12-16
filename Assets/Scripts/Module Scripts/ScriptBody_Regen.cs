using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptBody_Regen : ScriptBody_Default
{
	private float cooldown = 3;
	private float timeLeft = 3;
	private float heal = 0.5f;
	void Start()
    {
		parentCore = this.gameObject.transform.parent.GetComponent<ScriptCore>();
        STARTING_HEALTH = 8;
		
		if(health > STARTING_HEALTH) {
			health = STARTING_HEALTH;
			parentCore.TransmitHealth(health);
			parentCore.maxHealth = STARTING_HEALTH;
		}

		if (isLocalPlayerDerived)
		{
			MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
			render.enabled = false;
		}
	}
    
	void Update()
	{
		if( isLocalPlayerDerived )
		{
			timeLeft -= Time.deltaTime;
			if( timeLeft <= 0 )
			{
				if(health < STARTING_HEALTH)
				{
					GainHealth(heal);
					timeLeft = cooldown;
				}
				if(health > STARTING_HEALTH)
				{
					health = STARTING_HEALTH;
				}
				parentCore.TransmitHealth(health);
			}
		}
	}
}
