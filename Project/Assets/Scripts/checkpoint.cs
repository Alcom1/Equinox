using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public int checkNum;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        other.transform.parent.gameObject.GetComponent<PlayerScriptNN>().CheckCheckPoint(checkNum);
    }
}
