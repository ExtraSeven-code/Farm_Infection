using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Datos básicos")]
    public string id;
    public string displayName;
    public Sprite icon;

    [Header("Configuración")]
    public int maxStack = 64;
    public bool isConsumable;

    [Header("Prefab en el mundo 3D")]
    public GameObject worldPrefab;
}
