using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]

public class CraftingRecipe : ScriptableObject
{
    [System.Serializable]
    public class Ingredient
    {
        public ItemData item;
        public int amount = 1;
    }

    public string recipeName;
    public Ingredient[] ingredients;

    [Header("Resultado")]
    public ItemData resultItem;
    public int resultAmount = 1;
}
