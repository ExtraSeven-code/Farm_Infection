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
    public bool isSeed;                 // ✅ marcar si es una semilla
    public GameObject plantPrefab;      // planta que aparece en la maceta
    public ItemData harvestItem;        // fruto que dará al cosechar
    public int harvestAmount = 1;

    [Header("Herramienta (opcional)")]
    public bool isWateringTool;

    [Header("Colocables tipo bloque")]
    public bool isPlaceableBlock;         
    public GameObject placeablePrefab;

    public enum ToolType
    {
        None,
        Axe,
        Pickaxe,
        Hoe,
        scythe
    }

    [Header("Herramientas")]
    public bool isTool;
    public ToolType toolType;
    public int toolPower = 1;

    [Header("Modelo en la mano")]
    public GameObject handPrefab;

    [Header("Comida")]
    public bool isFood;
    public float restoreHealth;
    public float restoreStamina;
    public float restoreHunger;
    public float restoreSanity;
}
