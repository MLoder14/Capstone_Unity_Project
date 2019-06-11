using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTimer : MonoBehaviour
{
    float counter = 0.0f;
    float waitTime = 9.0f;
    bool disabled = false;

    void Update()
    {
        counter += Time.deltaTime;
        if(counter  >= waitTime && disabled == false)
        {
            disabled = true;
            gameObject.SetActive(false);
        }
    }
}
