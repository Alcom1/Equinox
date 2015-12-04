﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

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
			RectTransform rectTransform = GameObject.Find("Body Cooldown Bar").GetComponent<RectTransform>();
			
			//link shield activation to Q
			if (Input.GetKey(KeyCode.Q) && timeLeft <= 0)
			{
				shielding = duration;
				timeLeft = cooldown;
				//sync enabled
				parentCore.shieldsUp = true;
			}
			
			if( shielding <= Time.deltaTime ) {
				//shield fade animation
				//sync disabled
				parentCore.shieldsUp = false;
			}
			
			shielding -= Time.deltaTime;
			timeLeft -= Time.deltaTime;
			rectTransform.sizeDelta = new Vector2(1800*(timeLeft/cooldown),rectTransform.sizeDelta.y);
			
			if( timeLeft <= 0 )
			{
				//display ability can be actived
				rectTransform.sizeDelta = new Vector2(1800,rectTransform.sizeDelta.y);
			}
		}
	}
	
	public void LoseHealth(Component bulletScript, float damage)
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
