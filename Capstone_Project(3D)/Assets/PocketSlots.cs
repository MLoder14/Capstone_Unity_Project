using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add a function that keeps track of the slots and if they are both full, then update a bool that says full
/// 
/// add a function that returns the int of the free slot.
/// </summary>

public class PocketSlots : MonoBehaviour
{
    private GameObject[] slots = new GameObject[2];
    public bool full = false;

    //this is for the transforms
    public Transform[] slotPosition;
    
    public void AddtoPocket(GameObject go, int slotNum)
    {
        slots[slotNum] = go;
        /*
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
        */
    }

    /// <summary>
    /// Returns the int postion of the free slot.
    /// this allows us to take the free slots transform and use it to put the object away.
    /// </summary>
    /// <param name="posNumber"></param>
    /// <returns></returns>
    public Transform ReturnPosition(int posNumber)
    {
        return slotPosition[posNumber];
    }

    /// <summary>
    /// check each slot
    /// if they are both full return true
    /// </summary>
    /// <returns></returns>
    public int CheckSlots()
    {
        if (slots[0] == null)
        {
            return 0;
        }
        if (slots[1] == null)
        {
            return 1;
        }
        return -1;
    }
}
