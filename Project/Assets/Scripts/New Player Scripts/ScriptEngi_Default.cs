using UnityEngine;
using System.Collections;

public class ScriptEngi_Default : MonoBehaviour
{
    public Rigidbody rb;                        //Rigidbody of player

    public float maxRotateX;                    //maximum X axis rotation
    public float maxRotateY;                    //maximum Y axis rotation
    public int sensetivityRotateX;              //X axis rotation sensetivity
    public int sensetivityRotateY;              //Y axis rotation sensetivity
    public int maxSpeed;                        //Maximum speed
    public int accelRate;                       //Acceleration strength
    public float deaccelRate;                   //Active deacceleration magnitude

    private float lerpRate;                     //Scale (0-1) of lerping rigidbody velocity to forward

    // Use this for initialization
    void Start()
    {
        //Random spawning
        Vector3 start = GameObject.Find("start").transform.position;
        transform.position = new Vector3(start.x, start.y + Random.Range(-10, 10), start.z + Random.Range(-10, 10));
        transform.rotation = Quaternion.Euler(0, 90, 0);

        //Lerp rate
        lerpRate = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse x-axis yaw
        float rotate = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensetivityRotateX;
        if (rotate > maxRotateX)    //Limit rotation
            rotate = maxRotateX;
        else if (rotate < -maxRotateX)
            rotate = -maxRotateX;
        rb.AddRelativeTorque(Vector3.up * rotate);
        print(rotate);

        //Mouse y-axis pitch
        rotate = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensetivityRotateY;
        if (rotate > maxRotateY)    //Limit rotation
            rotate = maxRotateY;
        else if (rotate < -maxRotateY)
            rotate = -maxRotateY;
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
        if (Input.GetKey("w") && rb.velocity.magnitude < maxSpeed)
        {
            rb.AddRelativeForce(Vector3.forward * Time.deltaTime * accelRate);
        }
        else if (Input.GetKey("s"))
        {
            rb.AddForce(-rb.velocity * deaccelRate);
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
        print("Engi Collide");
        lerpRate = 0;
    }
}