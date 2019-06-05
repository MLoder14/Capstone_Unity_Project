using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter PickupEmittter;
    public FMODUnity.StudioEventEmitter PocketEmitter;
    public FMODUnity.StudioEventEmitter PlaceEmitter;
    // Start is called before the first frame update
    public void PlayPickup()
    {
        PickupEmittter.Play();
    }

    public void PlayPlace()
    {
        PlaceEmitter.Play();
    }

    public void PlayPocket()
    {
        PocketEmitter.Play();
    }
}
