using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoiseScript : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string InputNoises;
    FMOD.Studio.EventInstance NoiseEvent;

    private float timeToWait = 10.0f;
    private float currentTime = 0.0f;
    private bool finished = false;
    private bool started = false;
    private int[] values;
    


    /*private int DoorsValue;
    private int BellsValue;
    private int CreaksValue;
    private int LaughsValue;
    private int CupsValue;
    private int FirePlaceValue;
    private int MoneyValue;
    private int PaperValue;
    private int ScreamingValue;
    private int ToysValue;
    private int AssortedValue;*/

    // Start is called before the first frame update
    void Start()
    {
        NoiseEvent = FMODUnity.RuntimeManager.CreateInstance(InputNoises);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(NoiseEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
        NoiseEvent.start();
        values = new int[11];
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeToWait && started == false)
        {
            Debug.Log("Started sound coroutine");
            started = true;
            finished = false;
            StartCoroutine(PlaySound());
        }
        else if(currentTime >= timeToWait && finished == true)
        {
            Debug.Log("Ended sound coroutine");
            timeToWait = Random.Range(10.0f, 60.0f);
            currentTime = 0.0f;
            started = false;
            finished = false;
        }
    }

    IEnumerator PlaySound()
    {
        RandomizeValues();
        //FMODUnity.RuntimeManager.PlayOneShot(InputNoises, transform.position);
        NoiseEvent.start();

        yield return new WaitForSeconds(6);
        MuteAll();
        finished = true;
        yield break;
    }

    void RandomizeValues()
    {
        for (int i = 0; i < 11; i++)
        {
            values[i] = Random.Range(0, 2);
        }
        NoiseEvent.setParameterByName("DoorsParameter", values[0]);
        NoiseEvent.setParameterByName("BellsParameter", values[1]);
        NoiseEvent.setParameterByName("CreaksParameter", values[2]);
        NoiseEvent.setParameterByName("LaughsParameter", values[3]);
        NoiseEvent.setParameterByName("CupsParameter", values[4]);
        NoiseEvent.setParameterByName("FirePlaceParameter", values[5]);
        NoiseEvent.setParameterByName("MoneyParameter", values[6]);
        NoiseEvent.setParameterByName("PaperParameter", values[7]);
        NoiseEvent.setParameterByName("ScreamingParameter", values[8]);
        NoiseEvent.setParameterByName("ToysParameter", values[9]);
        NoiseEvent.setParameterByName("AssortedParameter", values[10]);
    }

    void MuteAll()
    {
        for (int i = 0; i < 11; i++)
        {
            values[i] = 0;
        }

        NoiseEvent.setParameterByName("DoorsParameter", values[0]);
        NoiseEvent.setParameterByName("BellsParameter", values[1]);
        NoiseEvent.setParameterByName("CreaksParameter", values[2]);
        NoiseEvent.setParameterByName("LaughsParameter", values[3]);
        NoiseEvent.setParameterByName("CupsParameter", values[4]);
        NoiseEvent.setParameterByName("FirePlaceParameter", values[5]);
        NoiseEvent.setParameterByName("MoneyParameter", values[6]);
        NoiseEvent.setParameterByName("PaperParameter", values[7]);
        NoiseEvent.setParameterByName("ScreamingParameter", values[8]);
        NoiseEvent.setParameterByName("ToysParameter", values[9]);
        NoiseEvent.setParameterByName("AssortedParameter", values[10]);
    }
}
