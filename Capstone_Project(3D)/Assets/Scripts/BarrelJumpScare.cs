using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelJumpScare : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rb.AddForce(0f, 0f, 3f, ForceMode.Impulse);
        }
    }
}
