using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject player;
    public Camera lockCamera;
    public Light lockLight;
    public LockController lockController;
    public GameObject lockObject;
    public Animator doorAnimator;
    private bool doorLocked = true;
    private bool playerPresent = false;
    private bool doorOpen = false;
    private bool currentlyLockpicking = false;

    private void Start()
    {
        lockLight.enabled = false;
        lockObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerPresent = true;
            Debug.Log("player entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerPresent = false;
            Debug.Log("player left");
        }
    }

    private void Update()
    {
        if (lockController.unlocked == true && doorLocked == true)
        {
            Debug.Log("door unlocked");
            doorLocked = false;
            player.SetActive(true);
            lockCamera.enabled = false;
            lockLight.enabled = false;
            lockObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E) && playerPresent == true)
        {
            if (doorLocked == true)
            {
                if(currentlyLockpicking == false)
                {
                    player.SetActive(false);
                    lockObject.SetActive(true);
                    lockCamera.enabled = true;
                    lockLight.enabled = true;
                    currentlyLockpicking = true;
                }
                else if(currentlyLockpicking == true)
                {
                    lockObject.SetActive(false);
                    player.SetActive(true);
                    lockCamera.enabled = false;
                    lockLight.enabled = false;
                    currentlyLockpicking = false;
                }
                
            }
            else if (doorLocked == false)
            {

                if (doorOpen == false)
                {
                    doorAnimator.SetBool("doorOpen", true);
                    doorOpen = true;
                }
                else
                {
                    doorAnimator.SetBool("doorOpen", false);
                    doorOpen = false;
                }

            }

        }
    }
}
