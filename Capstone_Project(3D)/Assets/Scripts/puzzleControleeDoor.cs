using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleControleeDoor : puzzleControlee
{
    private Vector3 startScale;

    private void Start()
    {
        
    }
    // Start is called before the first frame update
    public override void Activate()
    {
        base.Activate();
        gameObject.SetActive(false);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        gameObject.SetActive(true);
    }
}
