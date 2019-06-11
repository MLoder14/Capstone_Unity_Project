//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveMonster : MonoBehaviour
{
    public Transform[] locations;
    public GameObject monster;

    /// <summary>
    /// Calls move co-routine to move monster to starting position.
    /// </summary>
    /// <param name="location"></param>
    public void moveMonster(int location)
    {
        StartCoroutine(move(location));
    }

    /// <summary>
    /// Moves monster to starting location when called.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    IEnumerator move(int location)
    {
        monster.GetComponent<NavMeshAgent>().enabled = false;
        monster.GetComponent<ai_controller>().enabled = false;
        monster.transform.position = locations[location].position;
        yield return null;
        monster.GetComponent<NavMeshAgent>().enabled = true;
        monster.GetComponent<ai_controller>().enabled = true;
        monster.GetComponent<NavMeshAgent>().SetDestination(GameObject.Find("pTarget1").transform.position);
        yield break;
    }
}
