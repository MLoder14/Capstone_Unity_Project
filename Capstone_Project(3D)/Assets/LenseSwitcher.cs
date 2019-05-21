using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LenseSwitcher : MonoBehaviour
{
    public GameObject[] lights;

    //public Light spotlight; == 0
    //public Light dissolve_spotlight; == 1
    //public Light monster_spotlight; == 2

    private int activeLight = 0;
    private int currentLight = 0;

    // Start is called before the first frame update
    void Start()
    {
        lights[currentLight].SetActive(true);
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
}
