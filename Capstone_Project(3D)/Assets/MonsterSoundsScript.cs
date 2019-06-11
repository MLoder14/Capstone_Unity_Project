//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSoundsScript : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter FootstepEmitter;
    public FMODUnity.StudioEventEmitter AttackEmitter;

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// This function is meant to be called by events on the animator
    /// controller. Plays a footstep sound when called.
    /// </summary>
    public void PlayFootstep()
    {
        FootstepEmitter.Play();
    }

    /// <summary>
    /// This function is meant to be called by events on the animator
    /// controller. Plays an attack sound when called.
    /// </summary>
    public void playAttack()
    {
        AttackEmitter.Play();
    }
}
