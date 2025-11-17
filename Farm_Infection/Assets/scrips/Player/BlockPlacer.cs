using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance;
    public float gridSize = 1f;
    public LayerMask blockLayerMask; // capa de bloques, o dejar en "Everything"

    public GameObject previewInstance; // para el ghost opcional

    private HotbarSelector hotbarSelector;

    private void Awake()
    {
        hotbarSelector = FindObjectOfType<HotbarSelector>();
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Update()
    {
        if (hotbarSelector == null || InventoryManager.Instance == null)
            return;

        ItemData currentItem = hotbarSelector.GetSelectedItem();
        if (currentItem == null || !currentItem.isPlaceableBlock || currentItem.placeablePrefab == null)
        {
            if (previewInstance != null)
                previewInstance.SetActive(false);
            return;
        }

        // Calcular posición de la celda frente al jugador
        Vector3 placePos;
        Vector3 removePos;
        GetPlacementPositions(out placePos, out removePos);

        // Preview
        UpdatePreview(currentItem, placePos);

        // Click izquierdo -> colocar
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceBlock(currentItem, placePos);
        }

        // Click derecho -> quitar bloque
        if (Input.GetMouseButtonDown(1))
        {
            TryRemoveBlock(removePos);
        }
    }

    void GetPlacementPositions(out Vector3 placePos, out Vector3 removePos)
    {
        placePos = Vector3.zero;
        removePos = Vector3.zero;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(
        ray,
        out RaycastHit hit,
        maxDistance,
        blockLayerMask,
        QueryTriggerInteraction.Ignore))
        {
            // Posición para quitar (centro del bloque golpeado)
            Vector3 basePos = hit.collider.transform.position;
            removePos = basePos;

            // Posición para colocar (encima/delante del bloque golpeado)
            Vector3 pos = hit.point + hit.normal * (gridSize / 2f);
            pos /= gridSize;
            pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z)) * gridSize;
            placePos = pos;
        }
        else
        {
            // Si no golpea nada, colocar en un plano delante del jugador
            Vector3 fallback = playerCamera.transform.position + playerCamera.transform.forward * 3f;
            fallback /= gridSize;
            fallback = new Vector3(Mathf.Round(fallback.x), Mathf.Round(fallback.y), Mathf.Round(fallback.z)) * gridSize;
            placePos = fallback;
            removePos = placePos;
        }
    }

    void UpdatePreview(ItemData item, Vector3 placePos)
    {
        if (previewInstance == null)
        {
            previewInstance = Instantiate(item.placeablePrefab);
            // Material semi-transparente si quieres
        }

        previewInstance.SetActive(true);
        previewInstance.transform.position = placePos;
    }

    void TryPlaceBlock(ItemData item, Vector3 pos)
    {
        // Evitar colocar si ya hay algo muy cerca
        Collider[] colls = Physics.OverlapBox(pos, Vector3.one * (gridSize * 0.45f), Quaternion.identity, blockLayerMask);
        if (colls.Length > 0) return;

        GameObject go = Instantiate(item.placeablePrefab, pos, Quaternion.identity);
        go.layer = LayerMask.NameToLayer("Blocks"); // si usas una layer especial

        // Opcional: ajustar escala a gridSize como en el editor:
        MeshRenderer rend = go.GetComponentInChildren<MeshRenderer>();
        if (rend != null)
        {
            float size = rend.bounds.size.x;
            float scaleFactor = gridSize / size;
            go.transform.localScale = Vector3.one * scaleFactor;
        }

        // Consumir un ítem del stack
        int index = hotbarSelector.currentIndex;
        InventoryManager.Instance.RemoveItemFromSlot(true, index, 1);
    }

    void TryRemoveBlock(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapBox(pos, Vector3.one * (gridSize * 0.45f), Quaternion.identity, blockLayerMask);
        foreach (var c in colls)
        {
            Destroy(c.gameObject);
        }
    }
}
