using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string InputCompleteEvent;
    FMOD.Studio.EventInstance puzzleCompleteSound;

    private bool activated = false;

    //public GameObject[] puzzlePieces;
    public bool[] piecesInPlace;

    public GameObject affectedObject;
    // Start is called before the first frame update
    void Start()
    {
        puzzleCompleteSound = FMODUnity.RuntimeManager.CreateInstance(InputCompleteEvent);
        for(int i = 0; i < piecesInPlace.Length; i++)
        {
            piecesInPlace[i] = false;
        }
    }

    public void updatePiecesInPlaceTrue(int index)
    {
        bool allTrue = true;
        piecesInPlace[index] = true;
        for(int i = 0; i < piecesInPlace.Length; i++)
        {
            if(piecesInPlace[i] == false)
            {
                allTrue = false;
            }
        }
        if(allTrue == true)
        {
            puzzleCompleteSound.start();
            activated = true;
            affectedObject.GetComponent<puzzleControlee>().Activate();
        }
    }

    public void updatePiecesInPlaceFalse(int index)
    {
        piecesInPlace[index] = false;
        activated = false;
        affectedObject.GetComponent<puzzleControlee>().Deactivate();
    }


}
