using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class VoxelEditorData : MonoBehaviour
{
    public GameObject[] voxelPrefabs;
    public int selectedIndex = 0;
    public float gridSize = 1f;
    public bool editMode = true;

    [Header("Agrupación en jerarquía")]
    public Transform rootParent;
}
