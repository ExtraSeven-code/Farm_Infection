using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abrir_Inventario : MonoBehaviour
{
    public GameObject panelInventario;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            panelInventario.SetActive(!panelInventario.activeSelf);
        }
    }
}
