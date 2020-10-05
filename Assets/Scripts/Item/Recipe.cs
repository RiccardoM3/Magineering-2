using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Recipe : ScriptableObject
{

    public enum RecipeType {
        Crafting,
        Smelting,
        Crushing,
        PlateMaking,
        RodMaking,
        WireMaking
    }

    public Item item;
    public int amount;
    public List<SavedSlot> recipeItems = new List<SavedSlot>();
    public RecipeType type;
}
