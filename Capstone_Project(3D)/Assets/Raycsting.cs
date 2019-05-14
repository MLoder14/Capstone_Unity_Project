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

    //for the handslots transforms
    public GameObject[] HandSlots;

    //for the handslots items array
    public GameObject[] ItemSlots;

    private bool HandFull = false;

    private GameObject pouchObject;

    // Start is called before the first frame update
    void Start()
    {
        ItemSlots = new GameObject[2];
        //HandSlots = new GameObject[2];
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
                pouchObject = hit.collider.gameObject;

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


                        GameObject temp = ItemSlots[0];
                        ItemSlots[0] = null;
                        //temp.transform.position = pouch.GetComponent<PocketSlots>().ReturnPosition(slotNumber).position;
                        pouch.GetComponent<PocketSlots>().AddtoPocket(temp);
                        //temp.transform.SetParent(pouch.transform);
                        //HandSlots[0].gameObject.SetActive(false);
                        Destroy(temp);

                        //add the item to the itemslots array and remove it from the hand array.
                        //check if the add to pockets it true; if so add the item to that slot
                        //otherwise, do nothing.

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
                if (pouch != null)
                {
                    //pouch = hit.collider.gameObject.GetComponent<Animator>();
                    pouch.SetBool("Open", false);
                }

            }

            //Checks to see if it hit somthing that can be picked up. (puts item in hand)
            if (hit.collider.gameObject.tag == "PickupItem" && Input.GetKeyUp(KeyCode.O))
            {
                if (HandFull != true)
                {
                    item = hit.collider.gameObject;
                    //ItemSlots[0] = item;

                    GameObject temp = Instantiate(item.GetComponent<AltForm>().altForm, new Vector3(0, 0, 0), Quaternion.identity); ;
                    //item.transform.position = HandSlots[0].transform.position;
                    ItemSlots[0] = temp;
                    ItemSlots[0].transform.position = HandSlots[0].transform.position;
                    HandFull = true;
                    Destroy(item);

                    /*
                    if (item.transform.position == HandSlots[0].transform.position)
                    {
                        ItemSlots[0] = item;
                        HandFull = true;
                    }
                    */
                }

                //Transform itemPosition = item.GetComponent<Transform>();
                //itemPosition = HandSlots[0].GetComponent<Transform>();
                //HandSlots[1].transform;
            }

            //Drop item
            if (Input.GetKeyUp(KeyCode.U) && ItemSlots[0] != null)
            {
                ItemSlots[0].gameObject.GetComponent<Rigidbody>().isKinematic = false;
                ItemSlots[0].gameObject.GetComponent<Rigidbody>().useGravity = true;
            }

            //put into hands
            if (hit.collider.gameObject.tag == "PickupItem" && Input.GetKeyUp(KeyCode.I))
            {
                if (HandFull != true)
                {

                    item = hit.collider.gameObject;

                    
                    GameObject aTemp = Instantiate(item, new Vector3(0, 0, 0), Quaternion.identity);
                    //aTemp.transform.position = HandSlots[0].transform.position;

                    //Set the item slot of the hand to hold the pickeup item
                    ItemSlots[0] = aTemp;
                    ItemSlots[0].transform.position = HandSlots[0].transform.position;

                    //Removes the Item from the pockets array. DOES NOT delete or alter object
                    pouchObject.GetComponent<PocketSlots>().RemoveFromPocket(item);
                    //pouchObject.GetComponent<PocketSlots>().RemoveFromPocket(aTemp);

                    //update the player hand array
                    Destroy(item);

                }
            }
        }

    }

    public GameObject GetFromHand()
    {
        if (ItemSlots[0] != null)
        {
            GameObject temp = ItemSlots[0];
            ItemSlots[0] = null;
            return temp;
        }
        return null;
    }
}