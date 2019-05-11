using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterAttackScript : MonoBehaviour
{
    public bool damaging = false;
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
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
