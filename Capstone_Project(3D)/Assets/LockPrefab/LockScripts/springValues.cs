using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class springValues : MonoBehaviour
{
    public float scaleYMin;
    [HideInInspector]
    public float scaleYMax;
    [HideInInspector]
    public Vector3 startScale;
    public Vector3 startPosition;

    private void Start()
    {
        startScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        scaleYMax = startScale.y;
    }
}
