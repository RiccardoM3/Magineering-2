using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RecipeListController : MonoBehaviour
{
    public List<Recipe> unlockedRecipes = new List<Recipe>();

    public GameObject recipeListItemPrefab;
    public List<Recipe> recipeList = new List<Recipe>();
    public List<GameObject> recipeListItems = new List<GameObject>();
    public GameObject selectedRecipe = null;
    public GameObject requiredItemsList;

    // Start is called before the first frame update
    void Start()
    {
        CreateRecipes();
        UpdateRequireditems();
    }


    public void CreateRecipes()
    {
        foreach (Recipe recipe in recipeList)
        {
            GameObject recipeListItem = Instantiate(recipeListItemPrefab);
            recipeListItem.transform.SetParent(transform, false);
            recipeListItem.GetComponent<RecipeListItem>().LoadRecipe(recipe);
            recipeListItems.Add(recipeListItem);
        }

        selectedRecipe = recipeListItems[0];
    }

    public void UpdateRequireditems()
    {
        if (selectedRecipe != null)
        {
            DeleteRequiredItemsList();
            selectedRecipe.GetComponent<RecipeListItem>().LoadRequireditems(requiredItemsList);
        }
    }

    public void DeleteRequiredItemsList()
    {
        foreach (Transform child in requiredItemsList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SelectRecipe(GameObject recipeListItem)
    {
        selectedRecipe = recipeListItem;
        UpdateRequireditems();
    }

    public void Craft()
    {
        List<RequiredItemSlot> requiredItems = new List<RequiredItemSlot>(requiredItemsList.GetComponentsInChildren<RequiredItemSlot>());
        bool canCraft = true;
        foreach (RequiredItemSlot requiredItemSlot in requiredItems)
        {
            if (!requiredItemSlot.HasEnoughItems())
            {
                canCraft = false;
            }
        }

        if (canCraft)
        {
            Recipe recipe = selectedRecipe.GetComponent<RecipeListItem>().recipe;
            InventoryController.instance.AddToInventory(recipe.item, recipe.amount);

            foreach (RequiredItemSlot requiredItemSlot in requiredItems)
            {
                requiredItemSlot.SubtractRequiredAmount();
                InventoryController.instance.SubtractFromInventory(requiredItemSlot.item, requiredItemSlot.requiredAmount);
            }
        }
    }
}
