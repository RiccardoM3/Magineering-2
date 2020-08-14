using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public int stackAmount = 50;
    public List<Recipe> recipes = new List<Recipe>();
    public Sprite icon = null;
    //public List<Aspect> aspects = new List<Aspect>();         For future
    public List<SlotType> insertsInto = new List<SlotType>();

    public Recipe GetRecipe<RecipeType>() {
        for (int i = 0; i < this.recipes.Count; i++) {
            if (recipes[i].GetType() == typeof(RecipeType)) {
                return recipes[i];
            }
        }

        return null;
    }

    public virtual void Use()
    {
        //This method is designed to be overwritten
        //Debug.Log("using" + itemName);
    }

    public virtual void Hover()
    {
        //This method is designed to be overwritten
        //Debug.Log("Hovering:" + itemName);
    }

    public virtual void Unhover()
    {
        //This method is designed to be overwritten
        //Debug.Log("unHovering:" + itemName);
    }
}