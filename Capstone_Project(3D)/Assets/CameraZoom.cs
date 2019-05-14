using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera camera;
    public float startTime;
    public float zoomSpeed = 30.0f;
    bool timeSet = false;
    bool increasing = false;

    public float fieldOfViewMax = 60.0f;
    public float fieldOfViewMin = 30.0f;

    //public float t;
    public float duration = 100;

    // Start is called before the first frame update
    void Start()
    {

        camera = GetComponentInChildren<Camera>();
        camera.fieldOfView = fieldOfViewMax;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("was above: " + camera.transform.rotation.eulerAngles.x);
        //float t = (Time.time - startTime) / duration;
        if (camera.transform.rotation.eulerAngles.x >= 70 && camera.transform.rotation.eulerAngles.x <= 100 && camera.fieldOfView >= fieldOfViewMin)
        {
            
            camera.fieldOfView -= (zoomSpeed * Time.deltaTime);
        }
        else if(camera.fieldOfView <= fieldOfViewMax && (camera.transform.rotation.eulerAngles.x <= 70 || camera.transform.rotation.eulerAngles.x >= 270))
        {

            camera.fieldOfView += (zoomSpeed * Time.deltaTime);//Mathf.SmoothStep(fieldOfViewMax, fieldOfViewMin, t);
            //camera.fieldOfView = fieldOfViewMax;
        }

    }

    public void SetTime()
    {
        startTime = Time.time;
    }
}
