using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScriptBody_Shield : ScriptBody_Default
{
	private float cooldown = 20;
	private float timeLeft = 0;
	private float duration = 3;
	private float shielding = 0;
	
	void Start()
	{
		STARTING_HEALTH = 10;
		parentCore = this.gameObject.transform.parent.GetComponent<ScriptCore>();
		parentCore.maxHealth = STARTING_HEALTH;
		if (isLocalPlayerDerived)
		{
			MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
			render.enabled = false;
		}
		foreach (Transform child in transform)
        {
            if (child.tag == "Shield")
                child.GetComponent<MeshRenderer>().enabled = false;
        }
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
				isShielded = true;
				//sync enabled
				parentCore.shieldsUp = true;
				parentCore.TransmitShield(true);
				foreach (Transform child in transform)
				{
					if (child.tag == "Shield") {
						child.GetComponent<MeshRenderer>().enabled = true;
					}
				}
			}
			
			if( shielding <= Time.deltaTime && isShielded ) {
				//shield fade animation
				//sync disabled
				parentCore.shieldsUp = false;
				parentCore.TransmitShield(false);
				isShielded = false;
				Debug.Log("Shield Down");
				foreach (Transform child in transform)
				{
					if (child.tag == "Shield")
						child.GetComponent<MeshRenderer>().enabled = false;
				}
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
}
