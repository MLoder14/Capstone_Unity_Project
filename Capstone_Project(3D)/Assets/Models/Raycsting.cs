﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycsting : MonoBehaviour
{
    // access the collider object from the raycassting
    //use that to change that specific pouch bool.
    public GameObject pouch;
    //public Animator pouchAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        float theDistance;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, (forward), out hit));
        {
            theDistance = hit.distance;
            print(theDistance + " " + hit.collider.gameObject.name);

            if (hit.collider.gameObject.tag == "Pouch")
            {
                hit.collider.gameObject.GetComponent<Animator>().SetBool("Open", true);
            }
            else if (hit.collider.gameObject.tag != "Pouch")
            {
                hit.collider.gameObject.GetComponent<Animator>().SetBool("Open", false);
            }
        }

    }
}
