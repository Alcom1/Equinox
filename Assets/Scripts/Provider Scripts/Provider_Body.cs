using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Provider_Body : ProviderScript
{
    // Use this for initialization
    void Start()
    {
        GameObject visual = (GameObject)Instantiate(
            visualPrefab,
            this.transform.position,
            this.transform.rotation);
        visual.transform.Translate(new Vector3(0, 0, -.15f));
        visual.transform.localScale = new Vector3(
            visual.transform.localScale.x * 1.2f,
            visual.transform.localScale.y * 1.2f,
            visual.transform.localScale.z * 1.2f);
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
            other.transform.parent.GetComponent<ScriptCore>().bodyResource = objectName;
            other.transform.parent.GetComponent<ScriptCore>().GenerateBody(objectName);
			//GameObject.Find("NetManager").GetComponent<NetworkManagerCustom>().AddScript(this);
            //this.gameObject.SetActive(false);
			if(other.transform.parent.GetComponent<ScriptCore>().isLocalPlayer) {
				Spawn();
			}
        }
    }
}
