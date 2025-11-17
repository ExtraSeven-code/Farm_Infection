using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTableTrigger : MonoBehaviour
{
    public CraftingTable table;

    private void Reset()
    {
        if (table == null)
            table = GetComponentInParent<CraftingTable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var interaction = other.GetComponent<PlayerCraftingInteraction>();
        if (interaction != null)
            interaction.SetCurrentTable(table);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var interaction = other.GetComponent<PlayerCraftingInteraction>();
        if (interaction != null && interaction.CurrentTable == table)
            interaction.SetCurrentTable(null);
    }
}
