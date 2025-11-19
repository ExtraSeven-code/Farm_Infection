using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInit : MonoBehaviour
{
    void Start()
    {
        DynamicGI.UpdateEnvironment();
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.reflectionIntensity = 1f;


    }
}
