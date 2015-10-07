using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScriptNN : MonoBehaviour
{
    public Rigidbody rb;        //Rigidbody of player
    public GameObject projectile;
    public GameObject firingPoint;
    public float firingRate;
    public float bulletSpeed;
    private const int STARTING_HEALTH = 10;
    private int health = 10;

    private float lerpRate;     //Scale (0-1) of lerping rigidbody velocity to forward
    private int lap;            //Current lap
    public int Lap
    {
        get{ return lap; }
    }
    private int cp;
    public int Cp
    {
        get { return cp; }
    }
    public int cpCount;         //Current Checkpoint index

    //HUD elements
    private Text displayLap;

	// Use this for initialization
	void Start ()
    {
        //Random spawning
        Vector3 start = GameObject.Find("start").transform.position;
        transform.position = new Vector3(start.x, start.y + Random.Range(-10, 10), start.z + Random.Range(-10, 10));
        transform.rotation = Quaternion.Euler(0, 90, 0);

        lerpRate = 1;
        lap = -1;               //Lap starts at -1, before crossing finishline
        cp = 0;

        //Assign lap display and set it to 0.
        displayLap = GameObject.Find("TextLap").GetComponent<Text>();
        displayLap.text = "Lap: 0";
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

        //player sometimes rotates - not sure why
        else if (Input.GetKey("space") && firingRate <= 0)
        {
            // Instantiate the projectile at the position and rotation of this transform
            GameObject clone;
            //bullets are being made in the WRONG SPOT right now, also wrong rotation
            clone = (GameObject)Instantiate(
                projectile, 
                firingPoint.transform.position, 
                transform.rotation);
            // Give the cloned object an initial velocity along the current
            // object's Z axis
            clone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * bulletSpeed);

            firingRate = 0.4f;
        }

        //Decrement firing rate
        firingRate -= Time.deltaTime;
        
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

    //Checkpoint checking for checkpoints and lap incrementing
    public void CheckCheckPoint(int checkNum)
    {
        //If the met Checkpoint meets the current Checkpoint index
        if (cp == checkNum)
        {
            //If Checkpoint index is 0, that's the finish line, increment and displa lap
            if (cp == 0)
            {
                lap++;
                displayLap.text = "Lap: " + lap;                //UI display
                GetComponent<PlayerSyncLap>().TransmitLap(lap); //Transmit lap to other player
            }

            //Increment Checkpoint
            cp++;
            if(cp >= cpCount)
            {
                cp = 0;
            }
        }
    }

    public void LoseHealth()
    {
        health--;
        print("lost health!");
        if(health <= 0)
        {
            //do something
            print("respawn");
            health = STARTING_HEALTH;
        }
    }
}
