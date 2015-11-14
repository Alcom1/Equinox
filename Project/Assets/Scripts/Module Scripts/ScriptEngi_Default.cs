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
    public float angleDrag;                     //Angular drag
    public float moveDrag;                      //Movement drag

    private float lerpRate;                     //Scale (0-1) of lerping rigidbody velocity to forward

    // Use this for initialization
    void Start()
    {
        //Lerp rate
        lerpRate = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse x-axis yaw
        float rotate = Input.GetAxisRaw("Mouse X") * sensetivityRotateX;
        if (rotate > maxRotateX)    //Limit rotation
        {
            rotate = maxRotateX;
        }
        else if (rotate < -maxRotateX)
        {
            rotate = -maxRotateX;
        }
        rb.AddRelativeTorque(Vector3.up * rotate * Time.deltaTime);

        //Mouse y-axis pitch
        rotate = Input.GetAxisRaw("Mouse Y") * sensetivityRotateY;
        if (rotate > maxRotateY)    //Limit rotation
        {
            rotate = maxRotateY;
        }
        else if (rotate < -maxRotateY)
        {
            rotate = -maxRotateY;
        }
        rb.AddRelativeTorque(Vector3.right * rotate * Time.deltaTime);

        /*
        //Mouse click roll
        if (Input.GetMouseButton(0))
        {
            rb.AddRelativeTorque(Vector3.forward * Time.deltaTime * 60);
        }
        if (Input.GetMouseButton(1))
        {
            rb.AddRelativeTorque(-Vector3.forward * Time.deltaTime * 60);
        }
        */

        float temp = transform.parent.transform.rotation.eulerAngles.z;
        print(temp);

        //Auto roll
        if (temp > 180 && temp < 360)
        {
            rb.AddRelativeTorque(Vector3.forward * Time.deltaTime * 240 * (360 - temp) / 180);
        }
        if (temp > 0 && temp < 180)
        {
            rb.AddRelativeTorque(-Vector3.forward * Time.deltaTime * 240 * temp / 180);
        }

        //Key input for accelerate and deaccelerate
        if (Input.GetKey("w") && rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * Time.deltaTime * accelRate);
        }
        else if (Input.GetKey("s"))
        {
            rb.AddForce(-rb.velocity * deaccelRate);
        }

        //Drag
        rb.AddTorque(-rb.angularVelocity * angleDrag * Time.deltaTime);
        rb.AddForce(-rb.velocity * moveDrag * Time.deltaTime);

        //Increase lerprate back to 1f
        lerpRate += .4f * Time.deltaTime;
        if (lerpRate > 1)
            lerpRate = 1f;

        //Lerp velocity to forward direction
        rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * rb.velocity.magnitude, .5f * lerpRate);
    }

    //Sets lerping to zero to cause collision disorientation.
    public void Disorient()
    {
        lerpRate = 0;
    }
}