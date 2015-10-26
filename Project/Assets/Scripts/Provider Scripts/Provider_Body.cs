using UnityEngine;
using System.Collections;

public class Provider_Body : MonoBehaviour
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
        visual.transform.Translate(new Vector3(0, 0, -.15f));
        visual.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        visual.transform.parent = this.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        //Substitute action.
        //other.
    }
}
