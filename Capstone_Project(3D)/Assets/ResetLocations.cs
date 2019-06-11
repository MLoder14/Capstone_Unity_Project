using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLocations : MonoBehaviour
{
    public GameObject player;
    //public GameObject[] monsters;

    private Vector3 playerStartPosition;
    //private Vector3 playerStartRotationTarget;
    //private Vector3[] monsterStartPositions;
    //private Vector3[] monsterStartRotations;

    // Start is called before the first frame update
    void Start()
    {
        playerStartPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        //playerStartRotationTarget = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10);

        /*monsterStartPositions = new Vector3[monsters.Length];
        monsterStartRotations = new Vector3[monsters.Length];

        for (int i = 0; i < monsters.Length; i++)
        {
            monsterStartPositions[i] = new Vector3(monsters[i].transform.position.x, monsters[i].transform.position.y, monsters[i].transform.position.z);
            monsterStartRotations[i] = new Vector3(monsters[i].transform.rotation.eulerAngles.x, monsters[i].transform.rotation.eulerAngles.y, monsters[i].transform.rotation.eulerAngles.z);
        }*/
    }

    public void startResetPositions()
    {
        Debug.Log("called starter");
        StartCoroutine("resetPositions");
    }

    IEnumerator resetPositions()
    {
        Debug.Log("Called coroutine");
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        player.transform.position = playerStartPosition;
        yield return null;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

        /*for(int i = 0; i < monsters.Length; i++)
        {
            monsters[i].transform.position = monsterStartPositions[i];
            monsters[i].transform.eulerAngles = monsterStartRotations[i];
            monsters[i].GetComponent<ai_controller>().init();
        }*/
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
