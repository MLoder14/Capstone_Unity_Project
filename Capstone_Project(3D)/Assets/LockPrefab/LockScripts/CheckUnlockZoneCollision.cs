using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUnlockZoneCollision : MonoBehaviour
{
    public bool inUnlockZone;
    // Start is called before the first frame update
    void Start()
    {
        inUnlockZone = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision occurred");
        if (other.transform.tag == "unlockZone")
        {
            inUnlockZone = true;
            Debug.Log("inUnlockZone: " + inUnlockZone);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "unlockZone")
        {
            inUnlockZone = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
