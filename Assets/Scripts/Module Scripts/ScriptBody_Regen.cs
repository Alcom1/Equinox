using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptBody_Regen : ScriptBody_Default
{
	void Start()
    {
		health = 8;
        STARTING_HEALTH = health;
        displayHealth = GameObject.Find("TextHealth").GetComponent<Text>();
        displayHealth.text = "Health: " + health;
	}
    //dashing, strafing, dodging
}
