using UnityEngine;
using System.Collections;

public class ScriptWeap_Spray : ScriptWeap_Default
{
    public float sprayAngle;

    // Update is called once per frame
    void Update()
    {
        //Firing
        if (Input.GetKey("space") && firingRate <= 0 && isLocalPlayerDerived)
        {
            transform.parent.GetComponent<ScriptCore>().Boop(
                firingPoint.transform.position,
                transform.rotation * GetRandomDirection(sprayAngle));
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

    Quaternion GetRandomDirection(float coneAngle)
    {
        Quaternion tilt = Quaternion.AngleAxis(Random.Range(0, coneAngle), Vector3.up);

        Quaternion spin = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);

        return spin * tilt;
    }
}