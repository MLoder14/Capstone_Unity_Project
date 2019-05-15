using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycstingReesEdit : MonoBehaviour
{
    // access the collider object from the raycassting
    //use that to change that specific pouch bool.
    //public GameObject pouch;
    //public Animator pouchAnimator;

    private Animator pouch;
    private GameObject player; // for the player facing
    private GameObject item;
    private float currentTime = 0.0f;
    private float waitTime = 1.5f;
    private float timeIncrease = 0.1f;
    public bool timerOn = false;

    //for the handslots transforms
    public GameObject[] HandSlots;

    //for the handslots items array
    public GameObject[] ItemSlots;

    private bool HandFull = false;

    private GameObject pouchObject;


    void Start()
    {
        ItemSlots = new GameObject[2];
    }

    // Update is called once per frame
    void Update()
    {
        if(timerOn == true)
        {
            currentTime += timeIncrease;
            if(currentTime >= waitTime)
            {
                timerOn = false;
                currentTime = 0.0f;
            }
        }
        int layerMask = 1 << 9;
        layerMask = ~layerMask;
        RaycastHit hit;
        float theDistance;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);

        //if the ray cast hits something shoot it out in the debug log for testing.
        if (Physics.Raycast(transform.position, (forward), out hit, 2.0f, layerMask))
        {
            theDistance = hit.distance;
            //print(theDistance + " " + hit.collider.gameObject.name);

            //checks the ray cast to see if it hits the pouch.
            if (hit.collider.gameObject.tag == "Pouch")
            {
                //save the pouch animator for shutting it off later.
                //pouch = hit.collider.gameObject.GetComponentInParent<Animator>();
                pouch = hit.collider.gameObject.GetComponent<Animator>();
                PocketSlots pouchPocketSlots = hit.collider.gameObject.GetComponent<PocketSlots>();
                pouchObject = hit.collider.gameObject;

                if (Input.GetKeyUp(KeyCode.E))
                {
                    pouch.SetBool("Open", true);
                }

                if (pouch.GetBool("Open") == true && Input.GetKeyUp(KeyCode.E) && HandFull == true && pouch.GetCurrentAnimatorStateInfo(0).IsName("Idle 0"))//&& hit.collider.gameObject.tag == "Pouch")
                {

                    int slotNumber = pouchPocketSlots.CheckSlots();
                    if (slotNumber != -1)
                    {
                        Debug.Log("Getting in there");


                        GameObject temp = ItemSlots[0];
                        ItemSlots[0] = null;
                        pouchPocketSlots.AddtoPocket(temp);
                        //Destroy(temp);

                        //add the item to the itemslots array and remove it from the hand array.
                        //check if the add to pockets it true; if so add the item to that slot
                        //otherwise, do nothing.

                        HandFull = false;
                    }
                    else
                    {
                        Debug.Log("No empty HandSlots.");
                    }
                }
                //if the raycast doesn't hit the pouch, close it.
                //else if (hit.collider.gameObject.tag == "Pouch" && Input.GetKeyUp(KeyCode.R))
                else if (Input.GetKeyUp(KeyCode.R))
                {
                    if (pouch != null)
                    {
                        pouch.SetBool("Open", false);
                    }

                }

            }

            //Checks to see if it hit somthing that can be picked up. (puts item in hand)
            if (hit.collider.gameObject.tag == "PickupItem" && Input.GetKeyUp(KeyCode.E) && timerOn == false)
            {
                
                if (HandFull != true)
                {
                    if(hit.collider.transform.parent != null)
                    {
                        if (hit.collider.transform.parent.tag == "Pouch")
                        {
                            item = hit.collider.gameObject;
                            //Removes the Item from the pockets array. DOES NOT delete or alter object
                            item.GetComponentInParent<PocketSlots>().RemoveFromPocket(item);

                            //Set the item slot of the hand to hold the pickeup item
                            ItemSlots[0] = item;
                            ItemSlots[0].transform.position = HandSlots[0].transform.position;
                            ItemSlots[0].transform.SetParent(HandSlots[0].transform);
                            HandFull = true;
                        }
                        if (hit.collider.transform.parent.tag == "placementZone")
                        {
                            item = hit.collider.gameObject;
                            //Removes the Item from the pockets array. DOES NOT delete or alter object
                            item.GetComponentInParent<PlacePuzzlePiece>().removePiece();

                            //Set the item slot of the hand to hold the pickeup item
                            ItemSlots[0] = Instantiate(item.GetComponent<AltForm>().altForm);
                            ItemSlots[0].transform.position = HandSlots[0].transform.position;
                            ItemSlots[0].transform.SetParent(HandSlots[0].transform);

                            Destroy(item);
                            HandFull = true;
                        }
                    }
                    else
                    {
                        item = hit.collider.gameObject;

                        GameObject temp = Instantiate(item.GetComponent<AltForm>().altForm, new Vector3(0, 0, 0), Quaternion.identity);

                        ItemSlots[0] = temp;
                        ItemSlots[0].transform.position = HandSlots[0].transform.position;
                        ItemSlots[0].transform.SetParent(HandSlots[0].transform);
                        HandFull = true;
                        Destroy(item);
                    }
                    
                }
            }
        }

        //Drop item
        if (Input.GetKeyUp(KeyCode.R) && ItemSlots[0] != null)
        {
            ItemSlots[0].gameObject.GetComponent<Rigidbody>().isKinematic = false;
            ItemSlots[0].gameObject.GetComponent<Rigidbody>().useGravity = true;
            ItemSlots[0].transform.SetParent(null);
        }

    }

    public GameObject GetFromHand()
    {
        if (ItemSlots[0] != null)
        {
            GameObject temp = ItemSlots[0];
            ItemSlots[0] = null;
            HandFull = false;
            return temp;
        }
        return null;
    }
}