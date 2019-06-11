//Script Created By Rees Herbert
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

    /// <summary>
    /// Initializes values, instantiates and attaches FMOD sound event to parent object.
    /// </summary>
    void Start()
    {
        NoiseEvent = FMODUnity.RuntimeManager.CreateInstance(InputNoises);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(NoiseEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
        NoiseEvent.start();
        values = new int[11];
    }

    /// <summary>
    /// This function implements a timing functionality. If the current time has
    /// reached the time to wait variable it will play a sound. Waits until the
    /// sound has finished before determining a new random time to wait and begins
    /// counting again.
    /// </summary>
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeToWait && started == false)
        {
            //Debug.Log("Started sound coroutine");
            started = true;
            finished = false;
            StartCoroutine(PlaySound());
        }
        else if(currentTime >= timeToWait && finished == true)
        {
            //Debug.Log("Ended sound coroutine");
            timeToWait = Random.Range(45.0f, 120.0f);
            currentTime = 0.0f;
            started = false;
            finished = false;
        }
    }


    /// <summary>
    /// Plays randomized sound.
    /// </summary>
    /// <returns></returns>
    IEnumerator PlaySound()
    {
        RandomizeValues();
        NoiseEvent.start();

        yield return new WaitForSeconds(6);
        MuteAll();
        finished = true;
        yield break;
    }

    /// <summary>
    /// This function randomly enables or disables volume on event tracks to
    /// randomize the sound that will be output.
    /// </summary>
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

    /// <summary>
    /// This function mutes all tracks in the FMOD event.
    /// </summary>
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
