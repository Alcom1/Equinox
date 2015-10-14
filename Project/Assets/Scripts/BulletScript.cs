using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour
{
    public float lifeTime;          //Duration of the bullet.
    public float speed;             //Speed of the bullet.
    public Rigidbody rb;            //Bullet rigidbody
    private bool isColliding;       //True while colliding. Prevents double collisions.

	void Start ()
	{
        rb.velocity = transform.TransformDirection(Vector3.forward * speed);
    }

    //Updates every frame.
	void Update ()
	{
        isColliding = false;

        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isColliding)    //Do nothing if a collision is already in progress
            return;

        isColliding = true;

        if (
            other.tag == "Body" ||
            other.tag == "Engi" ||
            other.tag == "Weap")
        {
            foreach (Transform child in other.transform.parent)
            {
                if (child.tag == "Body")
                {
                    child.GetComponent<ScriptBody_Default>().LoseHealth(this);
                }
            }
        }
        else
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
}