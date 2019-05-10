using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{

    public GameObject FpsController;
    private bool RotationLock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this
        transform.position = Vector3.Lerp(transform.position, FpsController.transform.position, 0.5f);

        if (!Input.GetKey(KeyCode.Tab))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, FpsController.transform.rotation, 0.5f);
        }
    }

}
