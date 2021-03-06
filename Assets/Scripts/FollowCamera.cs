using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float Damping = 5.0f;

    void Update()
    {
        if (!Target)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, Target.position + Offset, Time.deltaTime * Damping);

        transform.LookAt(Target, Vector3.up);

    }
}
