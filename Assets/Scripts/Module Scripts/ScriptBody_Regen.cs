using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptBody_Regen : ScriptBody_Default
{
	protected float STARTING_HEALTH = 8;
	private float cooldown = 3;
	private float timeLeft = 3;
	private float heal = 0.5f;
	private ScriptCore parentCore;
	void Start()
    {
		health = 8;
        STARTING_HEALTH = health;
		
		//get the ScriptCore script
		parentCore = this.gameObject.transform.parent.GetComponent<ScriptCore>();
	}
    
	void Update()
	{
		if( isLocalPlayerDerived )
		{
			timeLeft -= Time.deltaTime;
			if( timeLeft <= 0 )
			{
				if(parentCore.health < STARTING_HEALTH)
				{
					parentCore.GainHealth(heal);
					timeLeft = cooldown;
				}
				if(parentCore.health > STARTING_HEALTH)
				{
					parentCore.health = STARTING_HEALTH;
				}
				parentCore.TransmitHealth(parentCore.health);
			}
		}
	}
}
