using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    [Header("Recetas disponibles en esta mesa")]
    public CraftingRecipe[] recipes;

    [Header("UI de crafteo")]
    public GameObject craftingPanel;   

    private void Start()
    {
        if (craftingPanel != null)
            craftingPanel.SetActive(false);
    }

    public void OpenUI()
    {
        if (craftingPanel != null)
            craftingPanel.SetActive(true);
    }

    public void CloseUI()
    {
        if (craftingPanel != null)
            craftingPanel.SetActive(false);
    }

    public void Craft(int recipeIndex)
    {
        if (recipeIndex < 0 || recipeIndex >= recipes.Length)
            return;

        CraftingRecipe recipe = recipes[recipeIndex];
        if (recipe == null) return;

        if (InventoryManager.Instance == null) return;

        if (!HasIngredients(recipe)) return;

        ConsumeIngredients(recipe);

        InventoryManager.Instance.AddItem(recipe.resultItem, recipe.resultAmount);
    }

    bool HasIngredients(CraftingRecipe recipe)
    {
        foreach (var ing in recipe.ingredients)
        {
            if (!InventoryHas(ing.item, ing.amount))
                return false;
        }
        return true;
    }

    bool InventoryHas(ItemData item, int amount)
    {
        int count = 0;

        foreach (var slot in InventoryManager.Instance.hotbar)
            if (slot.item == item) count += slot.quantity;

        foreach (var slot in InventoryManager.Instance.inventory)
            if (slot.item == item) count += slot.quantity;

        return count >= amount;
    }

    void ConsumeIngredients(CraftingRecipe recipe)
    {
        foreach (var ing in recipe.ingredients)
        {
            int remaining = ing.amount;

            remaining = RemoveFromList(InventoryManager.Instance.hotbar, ing.item, remaining);
            remaining = RemoveFromList(InventoryManager.Instance.inventory, ing.item, remaining);
        }
    }

    int RemoveFromList(List<InventorySlot> list, ItemData item, int amount)
    {
        for (int i = 0; i < list.Count && amount > 0; i++)
        {
            var slot = list[i];
            if (slot.item != item || slot.IsEmpty) continue;

            int remove = Mathf.Min(slot.quantity, amount);
            slot.quantity -= remove;
            amount -= remove;

            if (slot.quantity <= 0)
                slot.Clear();
        }
        return amount;
    }
}
