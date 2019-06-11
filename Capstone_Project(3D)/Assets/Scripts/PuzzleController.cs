//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string InputCompleteEvent;
    FMOD.Studio.EventInstance puzzleCompleteSound;

    private bool activated = false;

    public bool[] piecesInPlace;

    public GameObject affectedObject;

    /// <summary>
    /// Instantiates and FMOD event for sound, and initializes the members fields
    /// values of the class.
    /// </summary>
    void Start()
    {
        puzzleCompleteSound = FMODUnity.RuntimeManager.CreateInstance(InputCompleteEvent);
        for(int i = 0; i < piecesInPlace.Length; i++)
        {
            piecesInPlace[i] = false;
        }
    }

    /// <summary>
    /// This function is meant to be called by a puzzle piece script. Updates
    /// the pieces in place array to reflect the placement of a puzzle piece.
    /// Checks to see if all pieces are in place and calls Activate method/plays sound
    /// if true.
    /// </summary>
    /// <param name="index">Index of puzzle piece script calling method</param>
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

    /// <summary>
    /// This funciton is meant to be called by a puzzle piece script. Updates
    /// the pieces in place array to reflect the removal of a puzzle piece.
    /// calls Deactivate method on puzzle controlee.
    /// </summary>
    /// <param name="index">Index of puzzle piece script calling method</param>
    public void updatePiecesInPlaceFalse(int index)
    {
        piecesInPlace[index] = false;
        activated = false;
        affectedObject.GetComponent<puzzleControlee>().Deactivate();
    }


}
