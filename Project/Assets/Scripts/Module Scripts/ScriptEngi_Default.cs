using UnityEngine;
using System.Collections;

public class ScriptEngi_Default : MonoBehaviour
{
    public Rigidbody rb;                        //Rigidbody of player

    public float maxRotateX;                    //maximum X axis rotation
    public float maxRotateY;                    //maximum Y axis rotation
    public int sensetivityRotateX;              //X axis rotation sensetivity
    public int sensetivityRotateY;              //Y axis rotation sensetivity
    public int autoRollRate;                    //Rate of auto-roll
    public int maxSpeed;                        //Maximum speed
    public int accelRate;                       //Acceleration strength
    public float deaccelRate;                   //Active deacceleration magnitude
    public float angleDrag;                     //Angular drag
    public float moveDrag;                      //Movement drag
    public float lerpMultiplier;                //Multiplier for velocity lerping.
    public float bounceMultiplier;              //Multiplier for bounce effect.

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
        moveYaw();
        movePitch();
        moveRoll();
        moveThrust();
        moveManipulate();
    }

    //Left-Right movement
    void moveYaw()
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
    }

    //Up-Down movement
    void movePitch()
    {
        //Mouse y-axis pitch
        float rotate = Input.GetAxisRaw("Mouse Y") * sensetivityRotateY;
        if (rotate > maxRotateY)    //Limit rotation
        {
            rotate = maxRotateY;
        }
        else if (rotate < -maxRotateY)
        {
            rotate = -maxRotateY;
        }
        rb.AddRelativeTorque(Vector3.right * rotate * Time.deltaTime);
    }

    //Roll movement
    void moveRoll()
    {
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

        //Auto roll
        if (temp > 180 && temp < 360)
        {
            rb.AddRelativeTorque(Vector3.forward * Time.deltaTime * autoRollRate * (360 - temp) / 180);
        }
        if (temp > 0 && temp < 180)
        {
            rb.AddRelativeTorque(-Vector3.forward * Time.deltaTime * autoRollRate * temp / 180);
        }
    }

    //Forward-back movement
    void moveThrust()
    {
        //Key input for accelerate and deaccelerate
        if (Input.GetKey("w") && rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * Time.deltaTime * accelRate);
        }
        else if (Input.GetKey("s"))
        {
            rb.AddForce(-rb.velocity * deaccelRate);
        }
    }

    //Misc movement stuff. Drag, lerp, etc.
    void moveManipulate()
    {
        //Drag
        rb.AddTorque(-rb.angularVelocity * angleDrag * Time.deltaTime);
        rb.AddForce(-rb.velocity * moveDrag * Time.deltaTime);

        //Increase lerprate back to 1f
        lerpRate += .1f * Time.deltaTime;
        if (lerpRate > 1)
            lerpRate = 1f;

        //Lerp velocity to forward direction
        rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * rb.velocity.magnitude, lerpMultiplier * lerpRate);

        //Limit velocity to max speed after a single bounce. Prevents chaining bounces to get ludicrous speed.
        if(rb.velocity.magnitude > maxSpeed * bounceMultiplier)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed * bounceMultiplier;
        }
    }

    //Sets lerping to zero to cause collision disorientation.
    public void Disorient()
    {
        lerpRate = 0;
        rb.velocity *= bounceMultiplier;
    }
}