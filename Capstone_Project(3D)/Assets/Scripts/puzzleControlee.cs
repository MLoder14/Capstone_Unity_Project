//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleControlee : MonoBehaviour
{
    //This is a base class to be inherited by children classes
    //the Activate and Deactivate methods must be overridden by the
    //child class.

    /// <summary>
    /// This is a public function that will be called by a puzzle controller
    /// when a puzzle has been completed. The method is virtualized
    /// to allow child classes to define puzzle specific behavior
    /// that can be called by the generic Activate method.
    /// </summary>
    public virtual void Activate()
    {
        //Will be overidden.
    }

    /// <summary>
    /// This is a public function that will be called by a puzzle controller
    /// when a puzzle has been completed. The method is virtualized
    /// to allow child classes to define puzzle specific behavior
    /// that can be called by the generic Deactivate method.
    /// </summary>
    public virtual void Deactivate()
    {
        //Will be overidden.
    }
}
