using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target == null && Camera.main != null)
            target = Camera.main.transform;

        if (target == null) return;

        transform.LookAt(transform.position + target.forward);
    }
}
