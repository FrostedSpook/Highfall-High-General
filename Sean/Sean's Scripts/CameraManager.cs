using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform Target;

    Vector3 offset;

    void Start()
    {
        offset = Target.position - transform.position;
    }
    private void Update()
    {
        transform.position = Target.position - offset;
    }
}
