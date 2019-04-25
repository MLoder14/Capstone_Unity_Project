using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleSpring : MonoBehaviour
{

    public GameObject spring;
    public GameObject pinTop;
    private float startY;
    private float startScaleY;
    private bool pinMoved = false;
    private float currentDistFromOrigin;

    // Start is called before the first frame update
    void Start()
    {
        startY = pinTop.transform.position.y;
        startScaleY = spring.transform.localScale.y;
        currentDistFromOrigin = 0.0f;
    }
    private float calculateDistance()
    {
        return Mathf.Round(pinTop.transform.position.y - startY * 1000f)/1000f;
    }

    // Update is called once per frame
    void Update()
    {
        if(calculateDistance() != currentDistFromOrigin)
        {
            pinMoved = true;
            currentDistFromOrigin = calculateDistance();
        }
        if(pinMoved == true)
        {
            Debug.Log("current dist from origin: " + currentDistFromOrigin);
            spring.transform.localScale = spring.transform.localScale + new Vector3(0, 1, 0) * currentDistFromOrigin;
            pinMoved = false;
        }
        else
        {

        }
        
    }
}
