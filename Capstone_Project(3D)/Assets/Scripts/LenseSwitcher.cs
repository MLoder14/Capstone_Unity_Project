using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LenseSwitcher : MonoBehaviour
{
    public GameObject[] lights;
    public GameObject dissolveLight;

    //public Light spotlight; == 0
    //public Light dissolve_spotlight; == 1
    //public Light monster_spotlight; == 2

    private int activeLight = 0;
    private int currentLight = 0;

    // Start is called before the first frame update
    void Start()
    {
        lights[currentLight].SetActive(true);
        dissolveLight.transform.Rotate(new Vector3(0, 180, 0));
        //maskController.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NextLight();
        }
        if (currentLight != activeLight)
        {
            lights[activeLight].SetActive(false);
            lights[currentLight].SetActive(true);
            activeLight = currentLight;
        }
    }

    public void NextLight()
    {
        currentLight = (currentLight + 1) % lights.Length;
        if(currentLight == 2)// && dissolveLight.transform.rotation.y == 0)
        {
            dissolveLight.transform.Rotate(new Vector3(0,180,0));
            //enableMask();
        }
        else if(currentLight == 0)// && dissolveLight.transform.rotation.y == 180)
        {
            dissolveLight.transform.Rotate(new Vector3(0, 180, 0));
            //disableMask();
        }
    }

    public void PreviousLight()
    {
        if (currentLight == 0)
        {
            currentLight = lights.Length - 1;
        }
        else
        currentLight = (currentLight - 1);
    }

    private void enableMask()
    {
        //maskController.SetActive(true);
    }

    private void disableMask()
    {
       // maskController.SetActive(false);
    }
}
