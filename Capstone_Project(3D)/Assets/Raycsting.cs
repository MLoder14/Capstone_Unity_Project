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
    private GameObject player; // for the player facing
    private GameObject item;
    public GameObject[] Slots;

    private bool HandFull = false;
    //public GameObject BigItem

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        float theDistance;
       // HandFull = false;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);

        //if the ray cast hits something shoot it out in the debug log for testing.
        if (Physics.Raycast(transform.position, (forward), out hit))
        {
            theDistance = hit.distance;
            print(theDistance + " " + hit.collider.gameObject.name);

            //checks the ray cast to see if it hits the pouch.
            if (hit.collider.gameObject.tag == "Pouch")
            {
                //save the pouch animator for shutting it off later.
                pouch = hit.collider.gameObject.GetComponent<Animator>();
                hit.collider.gameObject.GetComponent<Animator>().SetBool("Open", true);

                if (Input.GetKeyUp(KeyCode.P))
                {
                    
                    GameObject temp = Instantiate(Slots[0].GetComponent<AltForm>().altForm, new Vector3 (0,0,0), Quaternion.identity);

                    int result = pouch.GetComponent<PocketSlots>().AddtoPocket(temp);

                    if (result != -1)
                    {
                        Destroy(Slots[0]);
                        Slots[0] = null;
                    }
                    else
                    {
                        Destroy(temp);
                    }
                    //GameObject handItem = Slots[0].gameObject;
                    //Slots[0].gameObject.SetActive(false);
                    //Slots[0] = null;

                }
            }
            //if the raycast doesn't hit the pouch, close it.
            else if (hit.collider.gameObject.tag != "Pouch")
            {
                if(pouch != null)
                {
                    //pouch = hit.collider.gameObject.GetComponent<Animator>();
                    pouch.SetBool("Open", false);
                }

            }
            
            //Checks to see if it hit somthing that can be picked up.
            if (hit.collider.gameObject.tag == "PickupItem" && Input.GetKeyUp(KeyCode.O))
            {
                if (HandFull != true)
                {
                    item = hit.collider.gameObject;
                    item.transform.position = Slots[0].transform.position;
                    if (item.transform.position == Slots[0].transform.position)
                    {
                        HandFull = true;
                    }
                }

                //Transform itemPosition = item.GetComponent<Transform>();
                //itemPosition = Slots[0].GetComponent<Transform>();
                //Slots[1].transform;
            }
        }

    }

    void UpdatePlayerFacing()
    {
        
    }
}
