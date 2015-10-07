using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScriptNN : MonoBehaviour
{
    public Rigidbody rb;                        //Rigidbody of player
    public bool isLocalPlayer;                  //If player is local
    public int STARTING_HEALTH = 10;            //Starting health

    private float lerpRate;                     //Scale (0-1) of lerping rigidbody velocity to forward

    private int health = 10;                    //Current health
    public int Health
    {
        get { return health; }
    }

    //HUD elements
    private Text displayHealth;

	// Use this for initialization
	void Start ()
    {
        //Random spawning
        Vector3 start = GameObject.Find("start").transform.position;
        transform.position = new Vector3(start.x, start.y + Random.Range(-10, 10), start.z + Random.Range(-10, 10));
        transform.rotation = Quaternion.Euler(0, 90, 0);

        //Lerp rate
        lerpRate = 1;

        //Assign lap display and set it to 0.
        displayHealth = GameObject.Find("TextHealth").GetComponent<Text>();
        displayHealth.text = "Health: " + health;
	}

	// Update is called once per frame
	void Update ()
    {
        //Mouse x-axis yaw
        float rotate = Input.GetAxisRaw("Mouse X") * Time.deltaTime * 120;
        if (rotate > 3)
            rotate = 3f;    //max rotate x
        rb.AddRelativeTorque(Vector3.up * rotate);

        //Mouse y-axis pitch
        rotate = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * 120;
        if (rotate > 3)
            rotate = 3f;    //max rotate y
        rb.AddRelativeTorque(Vector3.right * rotate);

        //Mouse click roll
        if (Input.GetMouseButton(0))
        {
            rb.AddRelativeTorque(Vector3.forward * Time.deltaTime * 60);
        }
        if (Input.GetMouseButton(1))
        {
            rb.AddRelativeTorque(-Vector3.forward * Time.deltaTime * 60);
        }

        //Key input for accelerate and deaccelerate
        if (Input.GetKey("w") && rb.velocity.magnitude < 20)
        {
            rb.AddRelativeForce(Vector3.forward * Time.deltaTime * 60 * 20);
        }
        else if (Input.GetKey("s"))
        {
            rb.AddForce(-rb.velocity * .8f);
        }
        
        //Drag
        rb.AddTorque(-rb.angularVelocity * .75f);
        rb.AddForce(-rb.velocity * .2f);

        //Increase lerprate back to 1f
        lerpRate += .4f * Time.deltaTime;
        if (lerpRate > 1)
            lerpRate = 1f;

        //Lerp velocity to forward direction
        rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * rb.velocity.magnitude, .5f * lerpRate);
	}

    //Stop lerping after collision.
    void OnCollisionEnter(Collision collision)
    {
        lerpRate = 0;
    }

    //Lose and display health
    public void LoseHealth()
    {
        if (isLocalPlayer)
        {
            health--;
            print("lost health!");
            if (health <= 0)
            {
                //do something
                print("respawn");
                health = STARTING_HEALTH;
            }

            displayHealth.text = "Health: " + health;                //UI display
            //GetComponent<PlayerSyncHealth>().TransmitHealth(health);       //Transmit lap to other player
        }
    }
}
