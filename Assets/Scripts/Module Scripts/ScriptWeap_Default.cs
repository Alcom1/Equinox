using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ScriptWeap_Default : NetworkBehaviour
{
    public bool isLocalPlayerDerived;           //Local player status derived from player core.
    public bool IsLocalPlayerDerived
    {
        set { isLocalPlayerDerived = value; }
    }

    public GameObject projectile;               //Projectile prefab.
    public GameObject[] firingPoints;              //Firing point
    protected int firingPointIndex;
    public float firingRate;                    //Firing rate
    protected float firingRateRecord;             //Firing rate returns to this after firing.

    // Use this for initialization
    void Start()
    {
        firingRateRecord = firingRate;
        firingPointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Firing
        if (Input.GetMouseButton(0) && firingRate <= 0 && isLocalPlayerDerived)
        {
            transform.parent.GetComponent<ScriptCore>().Boop(
                firingPoints[firingPointIndex].transform.position,
                transform.rotation);
            firingRate = firingRateRecord;

            firingPointIndex++;
            if (firingPointIndex >= firingPoints.Length)
                firingPointIndex = 0;
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