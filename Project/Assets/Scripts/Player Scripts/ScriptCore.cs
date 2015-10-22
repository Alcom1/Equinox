using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ScriptCore : NetworkBehaviour
{
    public Rigidbody rb;                    //Single rigidbody of the player.

    public GameObject bodyPrefab;           //Body prefab of the player.
    public GameObject engiPrefab;           //Engi prefab of the player.
    public GameObject weapPrefab;           //Weapon prefab of the player.

    private GameObject projectilePrefab;     //Projectile prefab derived from weapon.

    // Use this for initialization
    void Start()
    {
        //Disable collision forces on all non-local players. Collision physics should be client-side only.
        if (isLocalPlayer)
        {
            rb.isKinematic = false;

            Spawn();
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
    void Update()
    {

    }

    //Spawn or be at spawning position
    private void Spawn()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            foreach (GameObject player in players)
            {
                if (!player.GetComponent<ScriptCore>().isLocalPlayer)
                {
                    GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
                    float distance = 0;
                    float newDistance = 0;
                    int index = 0;
                    for (int i = 0; i < spawns.Length; i++)
                    {
                        newDistance = (player.transform.position - spawns[i].transform.position).magnitude;
                        if (newDistance > distance)
                        {
                            distance = 0;
                            index = i;
                        }
                    }
                    this.transform.position = spawns[index].transform.position;
                    this.transform.rotation = spawns[index].transform.rotation;
                }
            }
        }
        else
        {
            GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
            this.transform.position = spawns[0].transform.position;
            this.transform.rotation = spawns[0].transform.rotation;
        }
    }

    //Generates a new body module
    void GenerateBody(GameObject _bodyPrefab)
    {
        //Destroy old module.
        foreach (Transform child in transform)
        {
            if (child.tag == "Body")
                Destroy(child.transform);
        }

        //Instantiate new module.
        GameObject newBody = (GameObject)Instantiate(
            _bodyPrefab,
            this.transform.position,
            this.transform.rotation);
        newBody.GetComponent<ScriptBody_Default>().IsLocalPlayerDerived = isLocalPlayer;    //Set local player status of new body.
        newBody.GetComponent<ScriptBody_Default>().CheckCamera();                           //Disable camera if new body is not local.
        newBody.transform.parent = this.transform;                                          //Set new module as child of player core.
    }

    //Generates a new engi module
    void GenerateEngi(GameObject _engiPrefab)
    {
        //Destroy old module.
        foreach (Transform child in transform)
        {
            if (child.tag == "Engi")
                Destroy(child.transform);
        }

        //Instantiate new module.
        GameObject newEngi = (GameObject)Instantiate(
            _engiPrefab,
            this.transform.position,
            this.transform.rotation);
        newEngi.GetComponent<ScriptEngi_Default>().rb = this.GetComponent<Rigidbody>();     //Assign player core rigidbody to new engine.
        newEngi.transform.parent = this.transform;                                          //Set new module as child of player core.
    }

    //Generates a new weap module
    void GenerateWeap(GameObject _weapPrefab)
    {
        //Destroy old module.
        foreach (Transform child in transform)
        {
            if (child.tag == "Weap")
                Destroy(child.transform);
        }

        //Instantiate new module.
        GameObject newWeap = (GameObject)Instantiate(
            _weapPrefab,
            this.transform.position,
            this.transform.rotation);
        newWeap.GetComponent<ScriptWeap_Default>().IsLocalPlayerDerived = isLocalPlayer;    //Set local player status of new weapon.
        projectilePrefab = newWeap.GetComponent<ScriptWeap_Default>().projectile;           //Get projectile prefab from new weapon.
        newWeap.transform.parent = this.transform;                                          //Set new module as child of player core.
    }

    //Physics effects on collision.
    void OnCollisionEnter(Collision collision)
    {
        //Find engine and trigger disorientation.
        foreach (Transform child in transform)
        {
            if (child.tag == "Engi")
                child.GetComponent<ScriptEngi_Default>().Disorient();
        }
    }

    //Non-Client/Command call for Bullet spawn to prevent Network permission problems.
    public void Boop(Vector3 position, Quaternion rotation)
    {
        CmdSpawnBullet(position, rotation);
    }

    //Spawns a bullet at a given position and direction.
    [Command]
    private void CmdSpawnBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = (GameObject)Instantiate(
            projectilePrefab,
            position,
            rotation);

        NetworkServer.Spawn(bullet);
    }
}
