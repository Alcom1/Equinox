using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScriptCore : NetworkBehaviour
{
    public Rigidbody rb;                    //Single rigidbody of the player.
    
    [SyncVar (hook = "SyncBody")]
	public string bodyResource;

    [SyncVar(hook = "SyncEngi")]
    public string engiResource;

    [SyncVar(hook = "SyncWeap")]
    public string weapResource;
	
	[SyncVar (hook="SyncHealth")]
    public float health;                          //Current health
    public float maxHealth;
	
	[SyncVar]
	public bool shieldsUp = false;

    private GameObject projectilePrefab;     //Projectile prefab derived from weapon.

    // Use this for initialization
    void Start()
    {
        //Disable collision forces on all non-local players. Collision physics should be client-side only.
        if (isLocalPlayer)
        {
            rb.isKinematic = false;
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            Spawn();
        }

        //Stops the rigidbody from automatically generating things that makes the physics weird.
        rb.inertiaTensor = rb.inertiaTensor;
        rb.inertiaTensorRotation = rb.inertiaTensorRotation;
        rb.centerOfMass = rb.centerOfMass;
        
        //Generat default modules.
		GenerateBody(bodyResource);
		GenerateEngi(engiResource);
		GenerateWeap(weapResource);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Spawn or be at spawning position
    public void Spawn()
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
                            distance = newDistance;
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
    public void GenerateBody(string _bodyResource)
    {
		float health = 10;
        //Destroy old module.
        foreach (Transform child in transform)
        {
            if (child.tag == "Body")
            {
				health = child.GetComponent<ScriptBody_Default>().health;
                Destroy(child.gameObject);
            }
        }

        //Instantiate new module.
        GameObject newBody = (GameObject)Instantiate(
            Resources.Load("Modules/" + _bodyResource, typeof(GameObject)),
            this.transform.position,
            this.transform.rotation);
        newBody.GetComponent<ScriptBody_Default>().IsLocalPlayerDerived = isLocalPlayer;    //Set local player status of new body.
        newBody.GetComponent<ScriptBody_Default>().CheckCamera();                           //Disable camera if new body is not local.
        newBody.transform.parent = this.transform;                                          //Set new module as child of player core.
		
		if(maxHealth == 8) {
			health += 2;
		}
		maxHealth = newBody.GetComponent<ScriptBody_Default>().StartingHealth;
        if( health > maxHealth ) {
			health = maxHealth;
		}
		newBody.GetComponent<ScriptBody_Default>().health = health;
		
		RectTransform rectTransform = GameObject.Find("Body Cooldown Bar").GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(1800,rectTransform.sizeDelta.y);
		TransmitBody(bodyResource);
		TransmitHealth(health);
    }

    //Generates a new engi module
    public void GenerateEngi(string _engiResource)
    {
        //Destroy old module.
        foreach (Transform child in transform)
        {
            if (child.tag == "Engi")
                Destroy(child.gameObject);
        }

        //Instantiate new module.
        GameObject newEngi = (GameObject)Instantiate(
            Resources.Load("Modules/" + _engiResource, typeof(GameObject)),
            this.transform.position,
            this.transform.rotation);
		ScriptEngi_Default engiScript = newEngi.GetComponent<ScriptEngi_Default>();
		engiScript.rb = rb;//this.GetComponent<Rigidbody>();     //Assign player core rigidbody to new engine.
        newEngi.transform.parent = this.transform;                                          //Set new module as child of player core.

        TransmitEngi(engiResource);
    }

    //Generates a new weap module
    public void GenerateWeap(string _weapResource)
    {
        //Destroy old module.
        foreach (Transform child in transform)
        {
            if (child.tag == "Weap")
                Destroy(child.gameObject);
        }

        //Instantiate new module.
        GameObject newWeap = (GameObject)Instantiate(
            Resources.Load("Modules/" + _weapResource, typeof(GameObject)),
            this.transform.position,
            this.transform.rotation);
        newWeap.GetComponent<ScriptWeap_Default>().IsLocalPlayerDerived = isLocalPlayer;    //Set local player status of new weapon.
        projectilePrefab = newWeap.GetComponent<ScriptWeap_Default>().projectile;           //Get projectile prefab from new weapon.
        newWeap.transform.parent = this.transform;                                          //Set new module as child of player core.

        TransmitWeap(weapResource);
    }

    //Physics effects on collision.
    void OnCollisionEnter(Collision collision)
    {
        //this isn't running, no clue why
        Debug.Log("Colliding on script");
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

		bullet.GetComponent<BulletScript>().senderID = this.netId.ToString();

        NetworkServer.Spawn(bullet);
    }
    
    //Server set model
    [Command]
    void CmdSendBodyToServer(string body)
    {
        //runs on server, we call on client
        bodyResource = body;
    }

    [Client]
    void TransmitBody(string body)
    {
        // This is where we (the client) send out our prefabs.
        if (isLocalPlayer)
        {
            // Send a command to the server to update our health, and 
            // it will update a SyncVar, which then automagically updates on everyone's game instance
            CmdSendBodyToServer(body);
        }
    }

    //Sync model
    [Client]
    void SyncBody(string body)
    {
        //if there are changes, generate as necessary
        if (!body.Equals(bodyResource))
        {
            //generate based on name
            GenerateBody(body);
            bodyResource = body;
        }
    }

    //Server set model
    [Command]
    void CmdSendEngiToServer(string engi)
    {
        //runs on server, we call on client
        engiResource = engi;
    }

    [Client]
    void TransmitEngi(string engi)
    {
        // This is where we (the client) send out our prefabs.
        if (isLocalPlayer)
        {
            // Send a command to the server to update our health, and 
            // it will update a SyncVar, which then automagically updates on everyone's game instance
            CmdSendEngiToServer(engi);
        }
    }

    //Sync model
    [Client]
    void SyncEngi(string engi)
    {
        //if there are changes, generate as necessary
        if (!engi.Equals(engiResource))
        {
            //generate based on name
            GenerateEngi(engi);
            engiResource = engi;
        }
    }

    //Server set model
    [Command]
    void CmdSendWeapToServer(string weap)
    {
        //runs on server, we call on client
        weapResource = weap;
    }

    [Client]
    void TransmitWeap(string weap)
    {
        // This is where we (the client) send out our prefabs.
        if (isLocalPlayer)
        {
            // Send a command to the server to update our health, and 
            // it will update a SyncVar, which then automagically updates on everyone's game instance
            CmdSendWeapToServer(weap);
        }
    }

    //Sync model
    [Client]
    void SyncWeap(string weap)
    {
        //if there are changes, generate as necessary
        if (!weap.Equals(weapResource))
        {
            //generate based on name
            GenerateWeap(weap);
            weapResource = weap;
        }
    }
	
	//pass along to body script
	public void LoseHealth(Component bulletScript, float damage)
    {
		foreach (Transform child in transform)
        {
            if (child.tag == "Body")
            {
				child.GetComponent<ScriptBody_Default>().LoseHealth(bulletScript,damage);
            }
        }
	}
	
	    //Server set new health
    [Command]
    void CmdSendNewHealthToServer(float newHealth)
    {
        health = newHealth;
    }

    //Transmit new health to server
    [Client]
    public void TransmitHealth(float newHealth)
    {
        if (isLocalPlayer)// && CheckIfNewHealth(aHealth, newHealth))
        {
            GameObject.Find("TextHealth").GetComponent<Text>().text = "Health: " + newHealth;                //UI display
            CmdSendNewHealthToServer(newHealth);
        }
    }

    //Check if new health is different from old
    bool CheckIfNewHealth(float health1, float health2)
    {
        if (health1 == health2)
        {
            return false;
        }
        return true;
    }

    //Sync healths
    [Client]
    void SyncHealth(float newHealth)
    {
		health = newHealth;
		if (!isLocalPlayer)
        {
			GameObject.Find("TextHealthOpponent").GetComponent<Text>().text = "Opponent's Health: " + health;
		}
    }
}
