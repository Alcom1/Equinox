using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ScriptWeap_Default : NetworkBehaviour
{
    public bool isLocalPlayerDerived;

    public GameObject projectile;
    public GameObject firingPoint;              //Firing point
    public float firingRate;                    //Firing rate
    private float firingRateRecord;             //Firing rate returns to this after firing.

    // Use this for initialization
    void Start()
    {
        firingRateRecord = firingRate;
    }

    // Update is called once per frame
    void Update()
    {
        //Firing
        if (Input.GetKey("space") && firingRate <= 0 && isLocalPlayerDerived)
        {
            transform.parent.GetComponent<ScriptCore>().Boop(
                firingPoint.transform.position,
                transform.rotation);
            firingRate = firingRateRecord;
        }
        if (firingRate > 0)
        {
            firingRate -= Time.deltaTime;
        }
        else
        {
            firingRate = 0;
        }
    }
}