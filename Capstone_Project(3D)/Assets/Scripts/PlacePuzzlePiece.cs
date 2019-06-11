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

    public void removePiece()
    {
        piece = null;
        pieceInPlace = false;
        puzzleController.updatePiecesInPlaceFalse(pieceNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if(pieceInPlace == false && playerPresent && Input.GetKeyDown(KeyCode.E))
        {
            //piece = Instantiate(puzzlePiece, new Vector3(placementZone.transform.position.x, placementZone.transform.position.y, placementZone.transform.position.z), Quaternion.identity);
            GameObject temp = player.GetComponent<RaycstingReesEdit>().GetFromHand();
            if (temp != null)//player.GetComponentInChildren<RaycstingReesEdit>().ItemSlots[0] != null)
            {
                player.GetComponentInParent<PlayerSoundController>().PlayPlace();
                player.GetComponent<RaycstingReesEdit>().timerOn = true;
                //piece = Instantiate(player.GetComponentInChildren<RaycstingReesEdit>().ItemSlots[0].GetComponent<AltForm>().altForm, new Vector3(placementZone.transform.position.x, placementZone.transform.position.y, placementZone.transform.position.z), Quaternion.identity);
                piece = Instantiate(temp.GetComponent<AltForm>().altForm, new Vector3(placementZone.transform.position.x, placementZone.transform.position.y, placementZone.transform.position.z), Quaternion.identity);
                piece.transform.SetParent(placementZone.transform);
                Destroy(temp);
                pieceInPlace = true;
                puzzleController.updatePiecesInPlaceTrue(pieceNumber);
            }
        }
        /*else if(pieceInPlace == true && playerPresent && Input.GetKeyDown(KeyCode.E))
        {
            if(piece != null)
            {
                //Destroy(piece);
                if(piece.transform.parent != null)
                {
                    if(piece.transform.parent.tag == "Player")
                    {
                        piece = null;
                        pieceInPlace = false;
                        puzzleController.updatePiecesInPlaceFalse(pieceNumber);
                    }
                }
                
            }
                

        }*/
    }
}
