using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirrorTransform : MonoBehaviour
{
    public GameObject objectToMirror;
    private void OnPreRender()
    {
        transform.position = objectToMirror.transform.position;
        transform.eulerAngles = objectToMirror.transform.eulerAngles;
    }
}
