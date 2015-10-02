using UnityEngine;
using System.Collections;

public class checkpoint : MonoBehaviour
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
        other.transform.parent.gameObject.GetComponent<player_script_nn>().CheckCheckPoint(checkNum);
    }
}
