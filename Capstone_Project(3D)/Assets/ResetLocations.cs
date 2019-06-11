//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLocations : MonoBehaviour
{
    public GameObject player;

    private Vector3 playerStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerStartPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }

    /// <summary>
    /// Calls resetPositions coroutine to reset positions of the player.
    /// </summary>
    public void startResetPositions()
    {
        Debug.Log("called starter");
        StartCoroutine("resetPositions");
    }

    /// <summary>
    /// Resets the position of the player.
    /// </summary>
    /// <returns></returns>
    IEnumerator resetPositions()
    {
        Debug.Log("Called coroutine");
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        player.transform.position = playerStartPosition;
        yield return null;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
