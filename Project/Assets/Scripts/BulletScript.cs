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

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerScriptNN>().LoseHealth();
        }

        Destroy(this.gameObject);
    }
}