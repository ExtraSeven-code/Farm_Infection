using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPotTrigger : MonoBehaviour
{
    public PlantPot pot;   // referencia a la maceta (padre normalmente)

    private void Reset()
    {
        if (pot == null)
            pot = GetComponentInParent<PlantPot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var interaction = other.GetComponent<PlayerPlantInteraction>();
        if (interaction != null)
        {
            interaction.SetCurrentPot(pot);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var interaction = other.GetComponent<PlayerPlantInteraction>();
        if (interaction != null && interaction.CurrentPot == pot)
        {
            interaction.SetCurrentPot(null);
        }
    }
}
