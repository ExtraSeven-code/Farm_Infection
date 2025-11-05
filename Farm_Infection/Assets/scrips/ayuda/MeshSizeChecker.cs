using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSizeChecker : MonoBehaviour
{
    void Start()
    {
        var renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            Debug.Log($"{name} mesh size: {renderer.bounds.size}");
        }
    }
}
