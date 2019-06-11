//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterAttackScript : MonoBehaviour
{
    public bool damaging = false;
    public GameObject gameController;

    /// <summary>
    /// This function will be called by an event in the animator controller
    /// to set the damaging variable to true and enable damage to the player
    /// during the correct time during the animation.
    /// </summary>
    public void setDamagingTrue()
    {
        damaging = true;
    }

    /// <summary>
    /// This function will be called by an event in the animator controller
    /// to set the damaging variable to false and disable damage to the player.
    /// </summary>
    public void setDamagingFalse()
    {
        damaging = false;
    }

    /// <summary>
    /// This function checks if the player has been collided with by the ai hand
    /// colliders and checks if the damaging variable is set to true. If both are
    /// true the function calls a function representing the damage behavior.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && damaging == true)
        {
            Debug.Log("hit player.");
            gameController.GetComponent<ResetLocations>().startResetPositions();
        }
    }
}
