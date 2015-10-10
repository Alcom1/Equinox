using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour
{
    public float lifeTime;  //Duration of the bullet.
    public float speed;     //Speed of the bullet.
    public Rigidbody rb;    //Bullet rigidbody

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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerScriptNN>().LoseHealth(this);
        }
        else if (other.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }
}