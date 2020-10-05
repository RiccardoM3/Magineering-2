using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Recipe")]
public class Recipe : ScriptableObject
{

    public enum RecipeType {
        Crafting,
        Smelting,
        Crushing,
        PlateForming,
        RodForming,
        WireForming
    }

    public Item item;
    public int amount;
    public List<SavedSlot> recipeItems = new List<SavedSlot>();
    public RecipeType type;
}
