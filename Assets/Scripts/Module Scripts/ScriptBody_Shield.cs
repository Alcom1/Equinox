using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScriptBody_Shield : ScriptBody_Default
{
	private float cooldown = 20;
	private float timeLeft = 0;
	private float duration = 3f;
	private float shielding = 0;
	
	void Start()
	{
		STARTING_HEALTH = 10;
		RectTransform rectTransform = GameObject.Find("Health Bar").GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(180*STARTING_HEALTH,rectTransform.sizeDelta.y);
		parentCore = this.gameObject.transform.parent.GetComponent<ScriptCore>();
		if (isLocalPlayerDerived)
		{
			MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
			render.enabled = false;
		}
		
		GameObject.Find("Shield Ability").GetComponent<MeshRenderer>().enabled = false;
	}
	
	void Update()
	{
		if( isLocalPlayerDerived )
		{
			RectTransform rectTransform = GameObject.Find("Body Cooldown Bar").GetComponent<RectTransform>();
			
			//link shield activation to Q
			if (Input.GetKey(KeyCode.Q) && timeLeft <= 0)
			{
				Debug.Log("Shield Up");
				shielding = duration;
				timeLeft = cooldown;
				//sync enabled
				parentCore.shieldsUp = true;
				GameObject.Find("Shield Ability").GetComponent<MeshRenderer>().enabled = true;
			}
			
			if( shielding <= Time.deltaTime && parentCore.shieldsUp ) {
				//shield fade animation
				//sync disabled
				parentCore.shieldsUp = false;
				Debug.Log("Shield Down");
				GameObject.Find("Shield Ability").GetComponent<MeshRenderer>().enabled = false;
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
	
	public bool LoseHealth(Component bulletScript, float damage)
    {
		bool died = false;
        if (isLocalPlayerDerived && shielding <= 0)
        {
            health -= damage;
            print("lost health!");
            NetworkServer.Destroy(bulletScript.gameObject);
            if (health <= 0)
            {
                //do something
                parentCore.Spawn();
                health = STARTING_HEALTH;
				died = true;
            }
			
			parentCore.TransmitHealth(health);
        }
		return died;
    }
}
