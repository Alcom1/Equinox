using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour
{
    public float lifeTime;
    private float maxLifeTime;
    public float armTime;
    public float speed;
    public Rigidbody rb;

	void Start ()
	{
        rb.velocity = transform.TransformDirection(Vector3.forward * speed);
        maxLifeTime = lifeTime;
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
        if (other.tag == "Player" && maxLifeTime - lifeTime > armTime)
        {
            other.GetComponent<PlayerScriptNN>().LoseHealth(this);
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }
}