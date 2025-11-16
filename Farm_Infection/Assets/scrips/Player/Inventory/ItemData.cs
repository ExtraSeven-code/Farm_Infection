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

    [Header("Semilla / Planta (opcional)")]
    public bool isSeed;                 // ✅ marcar si este item es una semilla
    public GameObject plantPrefab;      // modelo de la planta en la maceta
    public ItemData harvestItem;        // fruto que dará al cosechar
    public int harvestAmount = 1;
}
