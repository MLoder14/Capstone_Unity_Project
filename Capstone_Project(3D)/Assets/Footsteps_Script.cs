//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps_Script : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string InputFootsteps;
    FMOD.Studio.EventInstance FootstepsEvent;

    bool playerismoving;
    public float walkingspeed;
    public float runSpeed;
    private float currentSpeed;
    private bool walkOn = true;
    private bool runOn = false;
    private float TileValue;
    private float LightValue;
    private float IceValue;
    private float SnowValue;
    private float RocksValue;
    private bool playerisgrounded;

    void Start()
    {
        FootstepsEvent = FMODUnity.RuntimeManager.CreateInstance(InputFootsteps);
        currentSpeed = walkingspeed;
        InvokeRepeating("CallFootsteps", 0, currentSpeed);
    }

    /// <summary>
    /// Check for player input, (movement and running) and player grounding.
    /// Plays footstep sounds if player is moving and grounded. Sets FMOD
    /// event parameters for volume of particular events. Values passed to 
    /// FMOD are determined by MaterialCheck script.
    /// </summary>
    void Update()
    {
        FootstepsEvent.setParameterByName("TileParameter", TileValue);
        FootstepsEvent.setParameterByName("LightParameter", LightValue);
        FootstepsEvent.setParameterByName("IceParameter", IceValue);
        FootstepsEvent.setParameterByName("SnowParameter", SnowValue);
        FootstepsEvent.setParameterByName("RocksParameter", RocksValue);

        if (Input.GetKey(KeyCode.LeftShift) && runOn == false && playerisgrounded == true)
        {
            walkOn = false;
            runOn = true;
            CancelInvoke();
            currentSpeed = runSpeed;
            InvokeRepeating("CallFootsteps", 0, currentSpeed);
            
        }
        else if(walkOn == false && !Input.GetKey(KeyCode.LeftShift) && playerisgrounded == true)
        {
            runOn = false;
            walkOn = true;
            CancelInvoke();
            currentSpeed = walkingspeed;
            InvokeRepeating("CallFootsteps", 0, currentSpeed);
            
        }

        if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f || Input.GetAxis("Horizontal") <= -0.01f)
        {
            if (playerisgrounded == true)
            {
                playerismoving = true;
            }
            else if (playerisgrounded == false)
            {
                playerismoving = false;
            }
        }
        else if (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
        {
            playerismoving = false;
        }
    }

    void CallFootsteps()
    {
        if (playerismoving == true)
        {
            FootstepsEvent.start();
        }
        else if (playerismoving == false)
        {
            //Debug.Log ("player is moving = false");
        }
    }

    void OnDisable()
    {
        playerismoving = false;
    }

    /// <summary>
    /// Checks what sound collider the player is colliding with, and if
    /// the player is grounded. Sets appropriate values based on this
    /// information.
    /// </summary>
    /// <param name="MaterialCheck"></param>
    void OnTriggerStay(Collider MaterialCheck)
    {
        playerisgrounded = true;

        if (MaterialCheck.CompareTag("Carpet:Material"))
        {
            LightValue = 1f;
            IceValue = 0f;
            SnowValue = 0f;
            TileValue = 0f;
            RocksValue = 0f;
        }
        if (MaterialCheck.CompareTag("Ice:Material"))
        {
            LightValue = 0f;
            IceValue = 1f;
            SnowValue = 0f;
            TileValue = 0f;
            RocksValue = 0f;
        }
        if (MaterialCheck.CompareTag("Snow:Material"))
        {
            LightValue = 0f;
            IceValue = 0f;
            SnowValue = 1f;
            TileValue = 0f;
            RocksValue = 0f;
        }
        if (MaterialCheck.CompareTag("Tile:Material"))
        {
            LightValue = 0f;
            IceValue = 0f;
            SnowValue = 0f;
            TileValue = 1f;
            RocksValue = 0f;
        }
        if (MaterialCheck.CompareTag("Rocks:Material"))
        {
            LightValue = 0f;
            IceValue = 0f;
            SnowValue = 0f;
            TileValue = 0f;
            RocksValue = 1f;
        }
    }

    /// <summary>
    /// Sets class variables to reflect player being in the air.
    /// </summary>
    /// <param name="MaterialCheck"></param>
    void OnTriggerExit(Collider MaterialCheck)
    {
        playerisgrounded = false;
        runOn = false;
        walkOn = false;
        LightValue = 0f;
        IceValue = 0f;
        SnowValue = 0f;
    }
}