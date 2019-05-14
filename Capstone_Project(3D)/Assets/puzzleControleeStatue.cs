using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleControleeStatue : puzzleControlee
{
    private Vector3 startScale;

    private void Start()
    {
        startScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }
    // Start is called before the first frame update
    public override void Activate()
    {
        base.Activate();
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        gameObject.transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z);
    }
}
