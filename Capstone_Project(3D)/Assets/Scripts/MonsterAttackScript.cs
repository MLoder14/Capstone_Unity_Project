using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterAttackScript : MonoBehaviour
{
    public bool damaging = false;
    public GameObject gameController;
    //doop

    public void setDamagingTrue()
    {
        damaging = true;
    }

    public void setDamagingFalse()
    {
        damaging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && damaging == true)
        {
            Debug.Log("hit player.");
            gameController.GetComponent<ResetLocations>().startResetPositions();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
