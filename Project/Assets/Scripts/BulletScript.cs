using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class BulletScript : NetworkBehaviour
{
    public float lifeTime;

	void Start ()
	{

	}

    //Updates every frame.
	void Update ()
	{
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(this.gameObject);
	}
}