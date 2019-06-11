using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Door_Script : MonoBehaviour
{
    private Animator animator;
    private bool PlayerPresent = false;

    public GameObject SoundEmitter;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerPresent == true)
        {
            animator.SetBool("Door_Open", true);
            SoundEmitter.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerPresent = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            PlayerPresent = false;
        }
    }

}
