using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSoundsScript : MonoBehaviour
{

    //[FMODUnity.EventRef]
    //public string InputFootsteps;
    //FMOD.Studio.EventInstance FootstepsEvent;
    //public string InputAttack;
    //FMOD.Studio.EventInstance AttackEvent;
    // Start is called before the first frame update
    public FMODUnity.StudioEventEmitter FootstepEmitter;
    public FMODUnity.StudioEventEmitter AttackEmitter;

    void Start()
    {
        //FootstepsEvent = FMODUnity.RuntimeManager.CreateInstance(InputFootsteps);
        //AttackEvent = FMODUnity.RuntimeManager.CreateInstance(InputAttack);
        //FootstepsEvent.start();
        //AttackEvent.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootstep()
    {
        FootstepEmitter.Play();
        //FMODUnity.RuntimeManager.PlayOneShot(InputFootsteps, transform.position);
    }

    public void playAttack()
    {
        AttackEmitter.Play();
    }
}
