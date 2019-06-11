using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture_BallRoom_Scare : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rb.isKinematic = false;
            rb.AddForce(10f, 0f, 0f, ForceMode.Impulse);
        }
    }

}
