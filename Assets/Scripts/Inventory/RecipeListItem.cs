using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeListItem : MonoBehaviour
{
    public Image icon;
    public new Text name;
    public Text amount;

    public Recipe recipe;
    public GameObject requiredItemPrefab;
    private RecipeListController recipeListController;

    // Start is called before the first frame update
    void Start()
    {
        recipeListController = transform.GetComponentInParent<RecipeListController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectRecipe()
    {
        recipeListController.SelectRecipe(gameObject);
    }

    public void LoadRecipe(Recipe loadedRecipe)
    {
        this.recipe = loadedRecipe;

        icon.sprite = loadedRecipe.item.icon;
        name.text = loadedRecipe.item.itemName;
        amount.text = loadedRecipe.amount.ToString();
    }

    public void LoadRequireditems(GameObject requiredItemsList)
    {
        foreach (SavedSlot requireditem in recipe.recipeItems)
        {
            GameObject requiredItemSlot = Instantiate(requiredItemPrefab);
            requiredItemSlot.transform.SetParent(requiredItemsList.transform);
            requiredItemSlot.GetComponent<RequiredItemSlot>().SetItem(requireditem);

            int amountInInventory = InventoryController.instance.inventoryContainer.countItems(requireditem.item);
            int amountInHotbar = InventoryController.instance.hotbarContainer.countItems(requireditem.item);

            requiredItemSlot.GetComponent<RequiredItemSlot>().SetInputtedAmount(amountInInventory + amountInHotbar);
        }
    }
}
