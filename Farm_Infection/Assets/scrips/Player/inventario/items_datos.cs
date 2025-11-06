using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class items_datos : ScriptableObject
{
    [Header("Datos del ítem")]
    public string itemName;
    public string description;

    [Header("Icono y modelo")]
    public Sprite icon;       
    public GameObject model3D; 

    [Header("Atributos")]
    public int maxStack = 10; 
}
