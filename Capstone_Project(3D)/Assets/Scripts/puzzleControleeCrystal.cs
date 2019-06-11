//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleControleeCrystal : puzzleControlee
{
    //child class of puzzleControlee
    private Vector3 startScale;

    private void Start()
    {
        
    }

    /// <summary>
    /// Overrides the base Activate class. Activates the parent object
    /// of this script when called.
    /// </summary>
    public override void Activate()
    {
        base.Activate();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Overrides the base Deactivate class. Deactivates the parent object
    /// of this script when called.
    /// </summary>
    public override void Deactivate()
    {
        base.Deactivate();
        gameObject.SetActive(false);
    }
}
