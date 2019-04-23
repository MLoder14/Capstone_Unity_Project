using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycsting : MonoBehaviour
{
    // access the collider object from the raycassting
    //use that to change that specific pouch bool.
    //public GameObject pouch;
    //public Animator pouchAnimator;

    private Animator pouch;
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

        //if the ray cast hits something shoot it out in the debug log for testing.
        if (Physics.Raycast(transform.position, (forward), out hit));
        {
            theDistance = hit.distance;
            print(theDistance + " " + hit.collider.gameObject.name);

            //checks the ray cast to see if it hits the pouch.
            if (hit.collider.gameObject.tag == "Pouch")
            {
                //save the pouch animator for shutting it off later.
                pouch = hit.collider.gameObject.GetComponent<Animator>();
                hit.collider.gameObject.GetComponent<Animator>().SetBool("Open", true);
            }
            //if the raycast doesn't hit the pouch, close it.
            else if (hit.collider.gameObject.tag != "Pouch")
            {
                pouch.SetBool("Open", false);
            }
        }

    }
}
