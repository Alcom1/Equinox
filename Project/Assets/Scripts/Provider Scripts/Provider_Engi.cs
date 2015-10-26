using UnityEngine;
using System.Collections;

public class Provider_Engi : MonoBehaviour
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
        visual.transform.localScale = new Vector3(2.4f, 2.4f, 2.4f);
        visual.transform.Translate(new Vector3(0, 0, 2.8f));
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
