using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(VoxelEditorData))]
public class VoxelEditorInspector : Editor
{
    private void OnSceneGUI()
    {
        VoxelEditorData data = (VoxelEditorData)target;
        if (!data.editMode || data.voxelPrefabs == null || data.voxelPrefabs.Length == 0)
            return;

        Event e = Event.current;
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Vector3 pos = GetMousePosition(data.gridSize);
        Handles.color = Color.green;
        Handles.DrawWireCube(pos, Vector3.one * data.gridSize);

        // 🟩 Clic izquierdo - colocar bloque
        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            PlaceVoxel(data, pos);
            e.Use();
        }

        // ❌ Clic derecho - borrar bloque
        if (e.type == EventType.MouseDown && e.button == 1 && !e.alt)
        {
            RemoveVoxel(pos, data.gridSize);
            e.Use();
        }
    }

    private Vector3 GetMousePosition(float gridSize)
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            hit /= gridSize;
            hit = new Vector3(Mathf.Round(hit.x), Mathf.Round(hit.y), Mathf.Round(hit.z)) * gridSize;
            return hit;
        }
        return Vector3.zero;
    }

    private void PlaceVoxel(VoxelEditorData data, Vector3 pos)
    {
        GameObject prefab = data.voxelPrefabs[data.selectedIndex];
        if (prefab == null) return;

        // 🧱 Evitar duplicados
        Collider[] colliders = Physics.OverlapBox(pos, Vector3.one * (data.gridSize * 0.45f));
        if (colliders.Length > 0) return;

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.transform.position = pos;

        // 🧩 Escala automática del bloque al tamaño del grid
        MeshRenderer rend = instance.GetComponentInChildren<MeshRenderer>();
        if (rend != null)
        {
            float size = rend.bounds.size.x; // asumimos que es un cubo
            float scaleFactor = data.gridSize / size;
            instance.transform.localScale = Vector3.one * scaleFactor;
        }
        else
        {
            instance.transform.localScale = Vector3.one * data.gridSize;
        }

        Undo.RegisterCreatedObjectUndo(instance, "Place Voxel");
    }

    private void RemoveVoxel(Vector3 pos, float gridSize)
    {
        Collider[] colliders = Physics.OverlapBox(pos, Vector3.one * (gridSize * 0.45f));
        foreach (Collider c in colliders)
        {
            Undo.DestroyObjectImmediate(c.gameObject);
        }
    }
}
