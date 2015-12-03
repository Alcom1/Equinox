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
						
						spawn = spawns[(int)(Random.Range(0,spawns.Length-.00001f))];
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
		//sync position
		TransmitPosition();
	}

    void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }
	
	//Server set position
	[Command]
	void CmdSendPositionToServer (Vector3 pos)
	{
		//runs on server, we call on client
		transform.position = pos;
	}
	
	[Client]
	void TransmitPosition ()
	{
		// Send a command to the server to update provider position
		CmdSendPositionToServer (transform.position);
	}

	//Sync position
	[Client]
	void SyncPositionValues (Vector3 latestPos)
	{
		transform.position = latestPos;
	}
}
