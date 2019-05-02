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
    public GameObject[] HandSlots;

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
                if (Input.GetKeyUp(KeyCode.E))
                {
                    hit.collider.gameObject.GetComponent<Animator>().SetBool("Open", true);
                }

                if (pouch.GetComponent<Animator>().GetBool("Open") == true && Input.GetKeyUp(KeyCode.P))//&& hit.collider.gameObject.tag == "Pouch")
                {

                    //GameObject temp = Instantiate(HandSlots[0].GetComponent<AltForm>().altForm, new Vector3 (0,0,0), Quaternion.identity);
                    //int result = pouch.GetComponent<PocketSlots>().AddtoPocket(temp);
                    int slotNumber = pouch.GetComponent<PocketSlots>().CheckSlots();
                    if (slotNumber != -1)
                    {                        
                        Debug.Log("Getting in there");

                        GameObject temp = Instantiate(HandSlots[0].GetComponent<AltForm>().altForm, new Vector3(0, 0, 0), Quaternion.identity);
                        temp.transform.position = pouch.GetComponent<PocketSlots>().ReturnPosition(slotNumber).position;
                        pouch.GetComponent<PocketSlots>().AddtoPocket(temp, slotNumber);
                        temp.transform.SetParent(pouch.transform);
                        //HandSlots[0].gameObject.SetActive(false);
                        Destroy(HandSlots[0]);
                        HandFull = false;
                    }
                    else
                    {
                        Debug.Log("No empty HandSlots.");
                        //Destroy(temp);
                    }
                }
            }

            //if the raycast doesn't hit the pouch, close it.
            else if (hit.collider.gameObject.tag == "Pouch" && Input.GetKeyUp(KeyCode.R))
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
                    item.transform.position = HandSlots[0].transform.position;
                    if (item.transform.position == HandSlots[0].transform.position)
                    {
                        HandSlots[0] = item;
                        HandFull = true;
                    }
                }

                //Transform itemPosition = item.GetComponent<Transform>();
                //itemPosition = HandSlots[0].GetComponent<Transform>();
                //HandSlots[1].transform;
            }

            if (hit.collider.gameObject.tag == "PickupItem" && Input.GetKeyUp(KeyCode.I))
            {
                if (HandFull != true)
                {
                    //create new bigger version of object inside pocket
                    GameObject aTemp = Instantiate(hit.collider.gameObject.GetComponent<AltForm>().altForm, new Vector3(0, 0, 0), Quaternion.identity);

                    //
                    HandSlots[0] = aTemp;

                    //move the position of the object to be in the players hand
                    aTemp.transform.position = HandSlots[0].transform.position;

                    //update the player hand array


                }
            }
        }

    }
}
