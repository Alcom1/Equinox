using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class player_script_nn : MonoBehaviour
{
    public Rigidbody rb;        //Rigidbody of player
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
    public int cpCount;         //Current checkpoint index

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
        if (Input.GetKey("w") && rb.velocity.magnitude < 60)
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

    //Checkpoint checking for checkpoints and lap incrementing
    public void CheckCheckPoint(int checkNum)
    {
        //If the met checkpoint meets the current checkpoint index
        if (cp == checkNum)
        {
            //If checkpoint index is 0, that's the finish line, increment and displa lap
            if (cp == 0)
            {
                lap++;
                displayLap.text = "Lap: " + lap;                //UI display
                GetComponent<PlayerSyncLap>().TransmitLap(lap); //Transmit lap to other player
            }

            //Increment checkpoint
            cp++;
            if(cp >= cpCount)
            {
                cp = 0;
            }
        }
    }
}
