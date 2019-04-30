using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketSlots : MonoBehaviour
{
    private GameObject[] slots;

    //this is for the transforms
    public GameObject[] slotPosition;

    public int AddtoPocket(GameObject go)
    {
        if (slots[0] == null)
        {
            slots[0] = go;
            return 0;
        }
        else if (slots[1] == null)
        {
            slots[1] = go;
            return 1;
        }
        return -1;
    }
    private void Update()
    {
        
    }
}
