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

    //for the item pocket gameobjects
    public GameObject[] pocketItems;

    public void Start()
    {
        pocketItems = new GameObject[2];
        Debug.Log("Pocket Items Length: " + pocketItems.Length);
    }

    /// <summary>
    /// make this potentially return a bool if it can add it to a pocket.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="slotNum"></param>
    public bool AddtoPocket(GameObject go)
    {
        int slotReturn = CheckSlots();
        //slots[slotNum] = go;
        if (slotReturn == -1)
        {
            return false;
        }
        if (slotReturn == 0)
        {
            //adding item to the pocket array
            //GameObject temp = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
            //Vector3 scale = temp.transform.lossyScale;
            pocketItems[slotReturn] = go;
            pocketItems[slotReturn].transform.position = slotPosition[slotReturn].position;
            pocketItems[slotReturn].transform.SetParent(slotPosition[slotReturn].transform);
            //pocketItems[slotReturn].transform.localScale = scale;
            //temp.SetActive(false);
            //Destroy(go);
            return true;
        }
        if (slotReturn == 1)
        {
            //adding item to the pocket array
            //GameObject temp = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
            //Vector3 scale = temp.transform.lossyScale;
            pocketItems[slotReturn] = go;
            pocketItems[slotReturn].transform.position = slotPosition[slotReturn].position;
            pocketItems[slotReturn].transform.SetParent(slotPosition[slotReturn].transform);
            //pocketItems[slotReturn].transform.localScale = scale;
            //pocketItems[slotReturn].transform.localScale = new Vector3(1, 1, 1);
            //temp.SetActive(false);
            //Destroy(go);
            return true;
        }
        return false;
    }

    /// <summary>
    /// take the item out of the pocket.
    /// call and just pass in the item you are removing from pouch
    /// </summary>
    /// <param name="go"></param>
    public GameObject RemoveFromPocket(GameObject go)
    {
        if (go == pocketItems[0])
        {
            GameObject temp = pocketItems[0];
            pocketItems[0] = null;
            return temp;
        }

        if (go == pocketItems[1])
        {
            GameObject temp = pocketItems[1];
            pocketItems[1] = null;
            return temp;
        }
        return null;
    }
    /// <summary>
    /// Returns the int position(transform) of the free slot.
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
        if (pocketItems[0] == null)
        {
            return 0;
        }
        if (pocketItems[1] == null)
        {
            return 1;
        }
        return -1;
    }
}