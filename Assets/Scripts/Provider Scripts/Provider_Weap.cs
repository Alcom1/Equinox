using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Provider_Weap : ProviderScript
{
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
        if (isColliding)    //Do nothing if a collision is already in progress
            return;

        isColliding = true;

        if (
            other.tag == "Body" ||
            other.tag == "Engi" ||
            other.tag == "Weap")
        {
            other.transform.parent.GetComponent<ScriptCore>().weapResource = objectName;
            other.transform.parent.GetComponent<ScriptCore>().GenerateWeap(objectName);
            //GameObject.Find("NetManager").GetComponent<NetworkManagerCustom>().AddScript(this);
            //this.gameObject.SetActive(false);
			Spawn();
        }
    }
}
