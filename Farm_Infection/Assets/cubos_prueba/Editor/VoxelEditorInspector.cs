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

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            PlaceVoxel(data, pos);
            e.Use();
        }

        if (e.type == EventType.MouseDown && e.button == 1 && !e.alt)
        {
            RemoveVoxel(pos, data.gridSize);
            e.Use();
        }
    }

    private Vector3 GetMousePosition(float gridSize)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (Event.current.button == 0)
            {
                Vector3 pos = hit.point + hit.normal * (gridSize / 2f);
                pos /= gridSize;
                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z)) * gridSize;
                return pos;
            }
            else if (Event.current.button == 1)
            {
                Vector3 pos = hit.point - hit.normal * (gridSize / 2f);
                pos /= gridSize;
                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z)) * gridSize;
                return pos;
            }
        }

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitpoint = ray.GetPoint(enter);
            hitpoint /= gridSize;
            hitpoint = new Vector3(Mathf.Round(hitpoint.x), Mathf.Round(hitpoint.y), Mathf.Round(hitpoint.z)) * gridSize;
            return hitpoint;
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

        // 📂 Asegurar carpeta raíz
        Transform root = data.rootParent;
        if (root == null)
        {
            GameObject existingRoot = GameObject.Find("VoxelRoot");
            if (existingRoot == null)
            {
                existingRoot = new GameObject("VoxelRoot");
                Undo.RegisterCreatedObjectUndo(existingRoot, "Create VoxelRoot");
            }

            root = existingRoot.transform;
            data.rootParent = root;
        }

        Transform typeFolder = root.Find(prefab.name);
        if (typeFolder == null)
        {
            GameObject folderGO = new GameObject(prefab.name);
            Undo.RegisterCreatedObjectUndo(folderGO, "Create Voxel Type Folder");
            folderGO.transform.SetParent(root);
            folderGO.transform.localPosition = Vector3.zero;
            folderGO.transform.localRotation = Quaternion.identity;
            folderGO.transform.localScale = Vector3.one;
            typeFolder = folderGO.transform;
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.transform.position = pos;
        instance.transform.SetParent(typeFolder); // ← lo metemos en su carpeta

        MeshRenderer rend = instance.GetComponentInChildren<MeshRenderer>();
        if (rend != null)
        {
            float size = rend.bounds.size.x; // asumimos cubo
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
