using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour
{
	public string senderID;			//ID of the player who fired the bullet.
    public float lifeTime;          //Duration of the bullet.
    public float speed;             //Speed of the bullet.
    public int damage;              //Damage dealt by the bullet.
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
            GameObject player = other.transform.parent.gameObject;
			if (player.tag == "Player")
			{
				player.GetComponent<ScriptCore>().LoseHealth(this, damage);
			}
        }
		else if(other.tag != "Bullet")
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
}