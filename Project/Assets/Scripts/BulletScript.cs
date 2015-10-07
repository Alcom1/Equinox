using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    public Rigidbody rb;

	void Start ()
	{
        rb.velocity = transform.TransformDirection(Vector3.forward * speed);
    }

    //Updates every frame.
	void Update ()
	{
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(this.gameObject);
	}
}