//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePuzzlePiece : MonoBehaviour
{
    private bool playerPresent = false;
    private bool pieceInPlace = false;
    private GameObject piece = null;

    public GameObject puzzlePiece;
    public GameObject placementZone;
    public GameObject player;
    public PuzzleController puzzleController;
    public int pieceNumber;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerPresent = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerPresent = false;
        }
    }

    /// <summary>
    /// Sets piece to null, and piece in place to false when the player
    /// has picked up a piece placed in the puzzle. Updates the puzzle 
    /// controller to reflect this change.
    /// </summary>
    public void removePiece()
    {
        piece = null;
        pieceInPlace = false;
        puzzleController.updatePiecesInPlaceFalse(pieceNumber);
    }

    /// <summary>
    /// Checks for player input and player presence, if both are true attempts to get an object
    /// from the player hand. If an object is present creates an instance of the object at the
    /// the placementZone and changes variables to reflect the placement of the piece.
    /// </summary>
    void Update()
    {
        if(pieceInPlace == false && playerPresent && Input.GetKeyDown(KeyCode.E))
        {
            GameObject temp = player.GetComponent<RaycstingReesEdit>().GetFromHand();
            if (temp != null)
            {
                player.GetComponentInParent<PlayerSoundController>().PlayPlace();
                player.GetComponent<RaycstingReesEdit>().timerOn = true;
                piece = Instantiate(temp.GetComponent<AltForm>().altForm, new Vector3(placementZone.transform.position.x, placementZone.transform.position.y, placementZone.transform.position.z), Quaternion.identity);
                piece.transform.SetParent(placementZone.transform);
                Destroy(temp);
                pieceInPlace = true;
                puzzleController.updatePiecesInPlaceTrue(pieceNumber);
            }
        }
    }
}
