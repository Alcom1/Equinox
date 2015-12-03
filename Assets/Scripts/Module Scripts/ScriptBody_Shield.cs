using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptBody_Regen : ScriptBody_Default
{
	private float cooldown = 20;
	private float timeLeft = 0;
	private float shielding = 3;
	private ScriptCore parentCore;
	void Start()
    {
		//get the ScriptCore script
		parentCore = this.gameObject.transform.parent.GetComponent<ScriptCore>();
	}
}
