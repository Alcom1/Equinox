using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ProviderScript : NetworkBehaviour
{
    public GameObject visualPrefab;
    public string objectName;
    protected bool isColliding;       //True while colliding. Prevents double collisions.

    void Update()
    {
        isColliding = false;
    }

	[Server]
	public void Spawn() {
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
					if(provider.transform == spawn.transform) {
						open = false;
						//remove the spawn from the array for efficiency
						
						spawn = spawns[(int)(Random.value*(spawns.Length-.00001))];
						break;
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

    void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }
}
