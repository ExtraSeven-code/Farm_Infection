using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlador_camara : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    [Range(0f, 1f)] public float startHeight = 0.5f; 

    void Start()
    {
        if (freeLookCam != null)
            freeLookCam.m_YAxis.Value = startHeight;
    }
}
