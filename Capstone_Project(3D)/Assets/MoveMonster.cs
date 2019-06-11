using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveMonster : MonoBehaviour
{
    public Transform[] locations;
    public GameObject monster;

    public void moveMonster(int location)
    {
        StartCoroutine(move(location));
    }

    IEnumerator move(int location)
    {
        monster.GetComponent<NavMeshAgent>().enabled = false;
        monster.GetComponent<ai_controller>().enabled = false;
        monster.transform.position = locations[location].position;
        //monster.GetComponent<ai_controller>().currentRoomNum = location;
        yield return null;
        monster.GetComponent<NavMeshAgent>().enabled = true;
        monster.GetComponent<ai_controller>().enabled = true;
        monster.GetComponent<NavMeshAgent>().SetDestination(GameObject.Find("pTarget1").transform.position);
        yield break;
    }
}
