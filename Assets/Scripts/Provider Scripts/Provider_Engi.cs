using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Provider_Engi : ProviderScript
{
    // Use this for initialization
    void Start()
    {
        GameObject visual = (GameObject)Instantiate(
            visualPrefab,
            this.transform.position,
            this.transform.rotation);
        visual.transform.localScale = new Vector3(2.4f, 2.4f, 2.4f);
        visual.transform.Translate(new Vector3(0, 0, 2.8f));
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
            other.transform.parent.GetComponent<ScriptCore>().engiResource = objectName;
            other.transform.parent.GetComponent<ScriptCore>().GenerateEngi(objectName);
            //GameObject.Find("NetManager").GetComponent<NetworkManagerCustom>().AddScript(this);
            //this.gameObject.SetActive(false);
			Spawn();
        }
    }
}
