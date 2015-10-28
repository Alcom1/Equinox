﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Provider_Body : NetworkBehaviour
{
    public GameObject visualPrefab;
    public string objectName;
    private bool isColliding;       //True while colliding. Prevents double collisions.

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

    void Update()
    {
        isColliding = false;
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
        }
    }
}
