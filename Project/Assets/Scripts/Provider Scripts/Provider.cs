using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Provider : NetworkBehaviour
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
        visual.transform.parent = this.transform;
        objectName = visualPrefab.ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        //Substitute action.
        //other.
        Debug.Log("script: " + FindObjectOfType<ScriptCore>().ToString());
        Debug.Log("prefab: " + visualPrefab.ToString());
        Debug.Log("name: "+objectName.ToString());
        Collider.FindObjectOfType<ScriptCore>().HandleModule(objectName);
    }
}
