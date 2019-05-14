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

    // Update is called once per frame
    void Update()
    {
        if(pieceInPlace == false && playerPresent && Input.GetKeyDown(KeyCode.E))
        {
            piece = Instantiate(puzzlePiece, new Vector3(placementZone.transform.position.x, placementZone.transform.position.y, placementZone.transform.position.z), Quaternion.identity);
            pieceInPlace = true;
            puzzleController.updatePiecesInPlaceTrue(pieceNumber);
        }
        else if(pieceInPlace == true && playerPresent && Input.GetKeyDown(KeyCode.E))
        {
            if(piece != null)
            {
                Destroy(piece);
                piece = null;
                pieceInPlace = false;
                puzzleController.updatePiecesInPlaceFalse(pieceNumber);
            }
                

        }
    }
}
