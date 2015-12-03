using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Provider_Body : NetworkBehaviour
{
    public GameObject visualPrefab;
    public string objectName;
    private bool isColliding;       //True while colliding. Prevents double collisions.
	private float maxCountdown = 5;
	private float countdown = 0;

    // Use this for initialization
    void Start()
    {
        GameObject visual = (GameObject)Instantiate(
            visualPrefab,
            this.transform.position,
            this.transform.rotation);
        visual.transform.Translate(new Vector3(0, 0, -.15f));
        visual.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        visual.transform.parent = this.transform;
    }

    void Update()
    {
        isColliding = false;
		if(!this.gameObject.activeSelf) {
			countdown -= Time.deltaTime;
			if(countdown <= 0) {
				countdown = 999;
				Spawn();
			}
		}
    }
	
	void Spawn() {
		GameObject[] providers = GameObject.FindGameObjectsWithTag("Provider");

        if (providers.Length > 1)
        {
			GameObject[] spawns = GameObject.FindGameObjectsWithTag("ProvSpawn");
			GameObject spawn = spawns[(int)(Random.value*(spawns.Length-0.00001))];
			
			bool open;
			do {
				open = true;
				foreach (GameObject provider in providers)
				{
					if (provider != this.gameObject)
					{
						if(provider.transform == spawn.transform) {
							open = false;
							//remove the spawn from the array for efficiency
							
							spawn = spawns[(int)(Random.value*(spawns.Length-.00001))];
							break;
						}
					}
				}
			} while(!open);
			this.transform.position = spawn.transform.position;
			this.gameObject.SetActive(true);
        }
        else
        {
            GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
            this.transform.position = spawns[0].transform.position;
            this.transform.rotation = spawns[0].transform.rotation;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (isColliding)    //Do nothing if a collision is already in progress
            return;

        isColliding = true;

        if (
            other.tag == "Body" ||
            other.tag == "Engi" ||
            other.tag == "Weap")
        {
            other.transform.parent.GetComponent<ScriptCore>().bodyResource = objectName;
            other.transform.parent.GetComponent<ScriptCore>().GenerateBody(objectName);
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }
}
