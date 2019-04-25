using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    //public enum pins {pin1,pin2,pin3,pin4};
    private int selectedPin;
    private int currentActivePin;
    public GameObject[] pinObjects;
    [HideInInspector]
    public bool[] pinsSet;
    [HideInInspector]
    public float necessaryMinTension;
    public float minTensionPerPin;
    public GameObject pickMovePoint;
    public GameObject wrenchMovePoint;
    public float wrenchMoveRate;

    private Vector3 pickPointStart;
    private Vector3 pickPointStartRotation;
    private Vector3 wrenchRotationStartEuler;
    public bool unlocked;
    private bool initializedActive;
    private MovePin[] pinControllers;
    

    // Start is called before the first frame update
    void Start()
    {
        wrenchRotationStartEuler = new Vector3(wrenchMovePoint.transform.eulerAngles.x, wrenchMovePoint.transform.eulerAngles.y, wrenchMovePoint.transform.eulerAngles.z);
        pickPointStart = new Vector3(pickMovePoint.transform.position.x, pickMovePoint.transform.position.y, pickMovePoint.transform.position.z);
        pickPointStartRotation = new Vector3(pickMovePoint.transform.eulerAngles.x, pickMovePoint.transform.eulerAngles.y, pickMovePoint.transform.eulerAngles.z);
        pinsSet = new bool[4];
        initializedActive = false;
        selectedPin = 0;
        currentActivePin = 0;
        pinObjects[selectedPin].GetComponent<MovePin>().pinActive = true;
        unlocked = false;
        pinControllers = new MovePin[4];
        for(int i = 0; i < 4; i++)
        {
            pinControllers[i] = pinObjects[i].GetComponent<MovePin>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pickMovePoint.transform.parent != pinControllers[selectedPin].pinBottom.transform)
        {
            movePickUnderPin();
        }
        if(pinControllers[selectedPin].pinActive == false && initializedActive == false)
        {
            pinControllers[selectedPin].pinActive = true;
            pinControllers[selectedPin].setHeightColorBlue();
            initializedActive = true;
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            selectedPin = Mathf.Clamp(selectedPin - 1, 0, 3);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (pinControllers[selectedPin].pinSet == true)
            {
                selectedPin = Mathf.Clamp(selectedPin + 1, 0, 3);
            }
        }
        //Debug.Log("SelectedPin: " + selectedPin + " CurrentActivePin: " + currentActivePin);
        if (unlocked == false)
        {
            if (pinsSet[0] == true && pinsSet[1] == true && pinsSet[2] == true && pinsSet[3] == true)
            {
                unlocked = true;
            }
        }
        if(selectedPin != currentActivePin)
        {
            pinControllers[currentActivePin].setHeightColorRed();
            pinControllers[currentActivePin].pinActive = false;
            pinControllers[selectedPin].pinActive = true;
            currentActivePin = selectedPin;
            pinControllers[currentActivePin].tensionWrenchForceCurrent = necessaryMinTension;
            pinControllers[currentActivePin].setHeightColorBlue();
            movePickUnderPin();
            //pickMovePoint.transform.position = new Vector3(pickPointStart.x, pickPointStart.y, pickPointStart.z);
        }
        if (pinControllers[currentActivePin].tensionWrenchForceCurrent < necessaryMinTension)
        {
            necessaryMinTension = 0;
            for (int i = 0; i < 4; i++)
            {
                if (i != currentActivePin)
                {
                    Debug.Log("Current active pin: " + currentActivePin.ToString());
                    pinsSet[i] = false;
                    pinControllers[i].StartCoroutine("failureMoveToStartPosition");
                    
                }
            }
        }
        rotateWrench();
        movePick();

    }

    public void calculateMinTension()
    {
        necessaryMinTension += minTensionPerPin;
        Debug.Log("necmintension: " + necessaryMinTension);
    }

    public void movePickUnderPin()
    {
        pickMovePoint.transform.position = new Vector3(pinObjects[currentActivePin].GetComponent<MovePin>().pinHeight.transform.position.x, pickPointStart.y, pickPointStart.z);
        pickMovePoint.transform.eulerAngles = new Vector3(pickPointStartRotation.x, pickPointStartRotation.y, 0.75f * selectedPin);
        pickMovePoint.gameObject.transform.SetParent(pinObjects[currentActivePin].GetComponent<MovePin>().pinBottom.transform);
        
    }

    public void movePick()
    {
        /*if(pinObjects[currentActivePin].GetComponent<MovePin>().pinCurMoveDist > 0)
        {
            pickMovePoint.transform.position = new Vector3(pickMovePoint.transform.position.x, pickPointStart.y + pinObjects[currentActivePin].GetComponent<MovePin>().pinCurMoveDist, pickMovePoint.transform.position.z);
        }
        else
        {
            pickMovePoint.transform.position = new Vector3(pickPointStart.x, pickPointStart.y, pickPointStart.z);
        }*/
        

    }

    public void rotateWrench()
    {
        wrenchMovePoint.transform.eulerAngles = new Vector3(wrenchRotationStartEuler.x + (pinObjects[currentActivePin].GetComponent<MovePin>().tensionWrenchForceCurrent/wrenchMoveRate), wrenchRotationStartEuler.y, wrenchRotationStartEuler.z);
    }
}
