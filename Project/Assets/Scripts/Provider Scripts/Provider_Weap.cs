using UnityEngine;
using System.Collections;

public class Provider_Weap : MonoBehaviour
{
    public GameObject visualPrefab;
    public string objectName;

    // Use this for initialization
    void Start()
    {
        GameObject visual = (GameObject)Instantiate(
            visualPrefab,
            this.transform.position,
            this.transform.rotation);
        visual.transform.localScale = new Vector3(.7f, .7f, .7f);
        visual.transform.parent = this.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        //Substitute action.
        //other.
        //these ALSO aren't working
        Debug.Log("name: " + objectName.ToString());
    }
}
