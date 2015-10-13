using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ScriptCore : NetworkBehaviour
{
    //~~
    public GameObject projectile;
    public GameObject firingPoint;              //Firing point
    public float firingRate;                    //Firing rate
    private float firingRateRecord;             //Firing rate returns to this after firing.
    //~~

    public Rigidbody rb;

    public GameObject bodyPrefab;
    public GameObject engiPrefab;
    public GameObject weapPrefab;

	// Use this for initialization
	void Start ()
    {
        //~~
        firingRateRecord = firingRate;
        //~~

        //Disable collision forces on all non-local players.
        if (isLocalPlayer)
        {
            rb.isKinematic = false;
        }

        //Stops the rigidbody from automatically generating things that makes the physics weird.
        rb.inertiaTensor = rb.inertiaTensor;
        rb.inertiaTensorRotation = rb.inertiaTensorRotation;
        rb.centerOfMass = rb.centerOfMass;

        //Generat default modules.
        GenerateBody(bodyPrefab);
        GenerateEngi(engiPrefab);
        GenerateWeap(weapPrefab);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //~~
        if (Input.GetKey("space") && firingRate <= 0 && isLocalPlayer)
        {
            CntSpawnBullet();
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
        //~~ 
    }

    //Generates a new body module
    void GenerateBody(GameObject _bodyPrefab)
    {
        foreach(Transform child in transform)
        {
            if (child.tag == "Body")
                Destroy(child.transform);
        }
        GameObject newBody = (GameObject)Instantiate(
            _bodyPrefab,
            this.transform.position,
            this.transform.rotation);
        newBody.GetComponent<ScriptBody_Default>().isLocalPlayerDerived = isLocalPlayer;
        newBody.GetComponent<ScriptBody_Default>().CheckCamera();
        newBody.transform.parent = this.transform;
        newBody.transform.localPosition = Vector3.zero;
        newBody.transform.localRotation = Quaternion.identity;
    }

    //Generates a new engi module
    void GenerateEngi(GameObject _engiPrefab)
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Engi")
                Destroy(child.transform);
        }
        GameObject newEngi = (GameObject)Instantiate(
            _engiPrefab,
            this.transform.position,
            this.transform.rotation);
        newEngi.GetComponent<ScriptEngi_Default>().rb = this.GetComponent<Rigidbody>();
        newEngi.transform.parent = this.transform;
    }

    //Generates a new weap module
    void GenerateWeap(GameObject _weapPrefab)
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Weap")
                Destroy(child.transform);
        }
        GameObject newWeap = (GameObject)Instantiate(
            _weapPrefab,
            this.transform.position,
            this.transform.rotation);
        firingPoint = newWeap.transform.Find("FiringPoint").gameObject;
        newWeap.transform.parent = this.transform;
    }

    //Stop lerping after collision.
    void OnCollisionEnter(Collision collision)
    {
        print("Collide");
        foreach (Transform child in transform)
        {
            if (child.tag == "Engi")
                child.GetComponent<ScriptEngi_Default>().Disorient();
        }
    }

    [Client]
    void CntSpawnBullet()
    {
        CmdSpawnBullet(firingPoint.transform.position, transform.rotation);
    }

    [Command]
    void CmdSpawnBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = (GameObject)Instantiate(
            projectile,
            position,
            rotation);

        NetworkServer.Spawn(bullet);
    }
}
