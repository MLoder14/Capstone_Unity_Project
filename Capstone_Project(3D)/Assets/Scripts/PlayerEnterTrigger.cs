using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterTrigger : MonoBehaviour
{
    public GameObject gameController;
    public int roomNum;
    public GameObject monster;
    ai_controller monsterController;

    private void Start()
    {
        monsterController = monster.GetComponent<ai_controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.tag == "Player" && monsterController.currentRoomNum != roomNum && monsterController.patrolling == true)
        {
            gameController.GetComponent<MoveMonster>().moveMonster(roomNum);
        }*/
    }
}
