using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps_Script : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string InputFootsteps;
    FMOD.Studio.EventInstance FootstepsEvent;
    //FMOD.Studio.ParameterInstance WoodParameter;
    //FMOD.Studio.ParameterInstance MetalParameter;
    //FMOD.Studio.ParameterInstance GrassParameter;

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
        //FootstepsEvent.getParameter("Wood", out WoodParameter);
        //FootstepsEvent.getParameter("Metal", out MetalParameter);
        //FootstepsEvent.getParameter("Grass", out GrassParameter);
        currentSpeed = walkingspeed;
        InvokeRepeating("CallFootsteps", 0, currentSpeed);
    }

    void Update()
    {
        //Debug.Log("TileValue: " + TileValue);
        FootstepsEvent.setParameterByName("TileParameter", TileValue);
        FootstepsEvent.setParameterByName("LightParameter", LightValue);
        FootstepsEvent.setParameterByName("IceParameter", IceValue);
        FootstepsEvent.setParameterByName("SnowParameter", SnowValue);
        FootstepsEvent.setParameterByName("RocksParameter", RocksValue);

        /*if(playerisgrounded == false)
        {
            CancelInvoke();
        }*/

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

    void OnTriggerStay(Collider MaterialCheck)
    {
        //float FadeSpeed = 10f;
        playerisgrounded = true;
        //Debug.Log("player is grounded");

        if (MaterialCheck.CompareTag("Carpet:Material"))
        {
            //Debug.Log("On Carpet Material");
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

    void OnTriggerExit(Collider MaterialCheck)
    {
        //Debug.Log("Left collider");
        playerisgrounded = false;
        runOn = false;
        walkOn = false;
        LightValue = 0f;
        IceValue = 0f;
        SnowValue = 0f;
        //CancelInvoke();
    }
}




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Footsteps_Script : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string inputSound;
    bool playerIsMoving;
    public float walkingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CallFootsteps", 0, walkingSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if(CrossPlatformInputManager.GetAxis("Horizontal") >= 0.1f || CrossPlatformInputManager.GetAxis("Horizontal") <= -0.1f || CrossPlatformInputManager.GetAxis("Vertical") >= 0.1f || CrossPlatformInputManager.GetAxis("Vertical") <= 0.1f)
        {
            playerIsMoving = true;
        }
        else if(CrossPlatformInputManager.GetAxis("Horizontal") == 0 || CrossPlatformInputManager.GetAxis("Vertical") == 0)
        {
            playerIsMoving = false;
        }
    }

    void CallFootsteps()
    {
        if(playerIsMoving == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(inputSound);
        }
    }

    private void OnDisable()
    {
        playerIsMoving = false;
    }
}
*/
