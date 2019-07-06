using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public float damp = 0.1f;
    public Transform followTarget;

    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 target = new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, damp);
    }
}
