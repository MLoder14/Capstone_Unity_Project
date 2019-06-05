using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePin : MonoBehaviour
{
    public GameObject spring;
    public GameObject pinTop;
    public GameObject pinBottom;
    public GameObject maxPinHeight;
    public GameObject minPinHeight;
    public GameObject pinOpenHeight;
    public GameObject pinHeight;
    public GameObject lockParent;

    [FMODUnity.EventRef]
    public string InputPinEvent = "event:/PinSet";
    FMOD.Studio.EventInstance pinSetEvent;
    
    public float tensionWrenchForceBuild;
    public float maxTension;
    public float minTension;
    public float frictionBuild;
    public float minFriction;
    public float springForceMultiplyer;
    public float springScaleBuild;
    public bool pinSet;
    public float movementResolution = 1;
    public int pinNumber;
    public float pinSuccessRotation;
    [HideInInspector]
    public float tensionWrenchForceCurrent = 1;
    [HideInInspector]
    public bool pinActive;
    [HideInInspector]
    private float springForce;

    private LockController lockController;
    private float pickForce;
    private float currentFriction = 1;
    private float maxFriction;
    private Vector3 startPosition;
    private Vector3 pinTopStartPos;
    private Vector3 pinBottomStartPos;
    private springValues springVals;
    private float pinMaxMoveDist;
    [HideInInspector]
    public float pinCurMoveDist;
    private Renderer pinHeightRenderer;
    private CheckUnlockZoneCollision unlockZoneCollision;
    

    // Start is called before the first frame update
    void Start()
    {
        pinSetEvent = FMODUnity.RuntimeManager.CreateInstance(InputPinEvent);
        lockController = lockParent.GetComponent<LockController>();
        pinHeightRenderer = pinHeight.GetComponent<Renderer>();
        unlockZoneCollision = pinHeight.GetComponent<CheckUnlockZoneCollision>();
        pinSet = false;
        pinActive = false;
        pinMaxMoveDist = maxPinHeight.transform.position.y - minPinHeight.transform.position.y;
        pickForce = pinMaxMoveDist / movementResolution;
        springForce = pickForce * springForceMultiplyer;
        springVals = spring.GetComponent<springValues>();
        maxFriction = maxTension * frictionBuild;
        currentFriction = 1;
        startPosition = new Vector3(pinHeight.transform.position.x, pinHeight.transform.position.y, pinHeight.transform.position.z);
        pinTopStartPos = new Vector3(pinTop.transform.position.x, pinTop.transform.position.y, pinTop.transform.position.z);
        pinBottomStartPos = new Vector3(pinBottom.transform.position.x, pinBottom.transform.position.y, pinBottom.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float moveForce = 0.0f;
        if (pinActive == true && pinSet == false)
        { 
            
            if (Input.GetKey(KeyCode.X))
            {
                tensionWrenchForceCurrent = Mathf.Clamp(tensionWrenchForceCurrent - tensionWrenchForceBuild, minTension, maxTension);
            }
            if (Input.GetKey(KeyCode.C))
            {
                tensionWrenchForceCurrent = Mathf.Clamp(tensionWrenchForceCurrent + tensionWrenchForceBuild, minTension, maxTension);
            }
            currentFriction = Mathf.Clamp(tensionWrenchForceCurrent * frictionBuild, minFriction, maxFriction);
            //Debug.Log("TensionWrenchForceCurrent: " + tensionWrenchForceCurrent + " currentFriction: " + currentFriction);
            if (Input.GetKey(KeyCode.Z))
            {
                if (pinHeight.transform.position.y < maxPinHeight.transform.position.y)
                {
                    if (pinHeight.transform.position.y + pickForce / currentFriction <= maxPinHeight.transform.position.y)
                    {
                        moveForce += pickForce / currentFriction;
                    }
                    else
                    {
                        moveForce += maxPinHeight.transform.position.y - pinHeight.transform.position.y;
                    }
                }

            }
            else if (pinHeight.transform.position.y > startPosition.y)
            {
                if (pinHeight.transform.position.y - springForce / currentFriction > startPosition.y)
                {
                    //pinHeight.transform.position = new Vector3(0, -springForce, 0) + pinHeight.transform.position;
                    //moveEntirePin(springForce);
                    moveForce -= springForce / currentFriction;
                }
                else
                {
                    //moveForce += startPosition.y - pinHeight.transform.position.y;
                    moveToStartPosition();
                }
            }
            if (currentFriction == maxFriction)
            {
                moveForce = 0.0f;
            }
            moveEntirePin(moveForce);
            
        }
        
        //if(Mathf.Abs(pinHeight.transform.position.y - pinOpenHeight.transform.position.y) <= successTolerance)
        if (unlockZoneCollision.inUnlockZone == true && pinSet == false)
        {
            pinHeightRenderer.material.color = Color.green;
            if(currentFriction == maxFriction)
            {

                //sound here
                pinSetEvent.start();
                

                pinSet = true;
                pinTop.transform.eulerAngles = new Vector3(pinTop.transform.eulerAngles.x - pinSuccessRotation, pinTop.transform.eulerAngles.y, pinTop.transform.eulerAngles.z);
                lockController.pinsSet[pinNumber] = true;
                lockController.calculateMinTension();
                //lockController.nextPin();
            }
        }
        else if(pinActive && pinHeightRenderer.material.color != Color.blue)
        {
            setHeightColorBlue();
        }
        /*else if(pinHeight.GetComponent<Renderer>().material.color != Color.red)
        {
            pinHeight.GetComponent<Renderer>().material.color = Color.red;
        }
        if(pinActive == true)
        {
            pinOpenHeight.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            pinOpenHeight.GetComponent<Renderer>().material.color = Color.white;
        }*/

    }

    void moveEntirePin(float inForce)
    {
        spring.transform.position = spring.transform.position + new Vector3(0, inForce / 2, 0);
        if (inForce != 0)
        {
            pinCurMoveDist = pinHeight.transform.position.y - minPinHeight.transform.position.y;
            float movePercentage = pinCurMoveDist / pinMaxMoveDist;
            float newScale = (springVals.scaleYMax - springVals.scaleYMin) * movePercentage;
            //spring.transform.localScale = new Vector3(springVals.startScale.x, Mathf.Clamp(spring.transform.localScale.y - springScaleBuild * Mathf.Sign(inForce)*(inForce/2), springVals.scaleYMin, springVals.scaleYMax), springVals.startScale.z);
            if (inForce > 0)
            {
                spring.transform.localScale = new Vector3(springVals.startScale.x, springVals.startScale.y - (newScale * Mathf.Sign(inForce)), springVals.startScale.z);
            }
            else
            {
                spring.transform.localScale = new Vector3(springVals.startScale.x, springVals.startScale.y - newScale, springVals.startScale.z);
            }
        }
        else if (pinHeight.transform.position == startPosition)
        {
            spring.transform.localScale = new Vector3(springVals.startScale.x, springVals.startScale.y, springVals.startScale.z);
        }
        pinTop.transform.position = pinTop.transform.position + new Vector3(0, inForce, 0);
        pinBottom.transform.position = pinBottom.transform.position + new Vector3(0, inForce, 0);
    }

    private float calculateSubdivSize()
    {
        return (maxPinHeight.transform.position.y - minPinHeight.transform.position.y) / movementResolution;
    }

    public IEnumerator failureMoveToStartPosition()
    {
        if (pinSet == true)
        {
            pinTop.transform.eulerAngles = new Vector3(pinTop.transform.eulerAngles.x + pinSuccessRotation, pinTop.transform.eulerAngles.y, pinTop.transform.eulerAngles.z);
        }
        currentFriction = minFriction;
        tensionWrenchForceCurrent = minTension;
        setHeightColorRed();
        if (pinHeight.transform.position.y > startPosition.y)
        {
            float moveForce = 0.0f;
            int counter = 0;
            while (pinHeight.transform.position.y > startPosition.y && counter < 100)
            {
                moveForce = 0.0f;
                if (pinHeight.transform.position.y - springForce / currentFriction > startPosition.y)
                {
                    moveForce -= springForce / currentFriction;
                    Debug.Log("move Force: " + moveForce.ToString());
                    moveEntirePin(moveForce);
                }
                else
                {
                    pinTop.transform.position = new Vector3(pinTopStartPos.x, pinTopStartPos.y, pinTopStartPos.z);
                    pinBottom.transform.position = new Vector3(pinBottomStartPos.x, pinBottomStartPos.y, pinBottomStartPos.z);
                    spring.transform.localScale = new Vector3(springVals.startScale.x, springVals.startScale.y, springVals.startScale.z);
                    spring.transform.position = new Vector3(springVals.startPosition.x, springVals.startPosition.y, springVals.startPosition.z);
                    Debug.Log("move Force: " + moveForce.ToString());
                }
                yield return null;
                counter++;
            }
            Debug.Log("setting pin = false");
            pinSet = false;
        }
        //Debug.Log("current friction: " + currentFriction.ToString() + "currentTension: " + tensionWrenchForceCurrent);
    }

    public void moveToStartPosition()
    {
        if (pinHeight.transform.position.y > startPosition.y)
        {
            pinTop.transform.position = new Vector3(pinTopStartPos.x, pinTopStartPos.y, pinTopStartPos.z);
            pinBottom.transform.position = new Vector3(pinBottomStartPos.x, pinBottomStartPos.y, pinBottomStartPos.z);
            spring.transform.localScale = new Vector3(springVals.startScale.x, springVals.startScale.y, springVals.startScale.z);
            spring.transform.position = new Vector3(springVals.startPosition.x, springVals.startPosition.y, springVals.startPosition.z);

            //yield return null;
        }
        //Debug.Log("current friction: " + currentFriction.ToString() + "currentTension: " + tensionWrenchForceCurrent);
    }

    public void setHeightColorBlue()
    {
        pinHeightRenderer.material.color = Color.blue;
    }

    public void setHeightColorRed()
    {
        pinHeightRenderer.material.color = Color.red;
    }
}
