//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleControleeDoor : puzzleControlee
{
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
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Overrides the base Deactivate class. Deactivates the parent object
    /// of this script when called.
    /// </summary>
    public override void Deactivate()
    {
        base.Deactivate();
        gameObject.SetActive(true);
    }
}
