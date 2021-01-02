using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeListItem : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;

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

    public void LoadRecipe(Recipe loadedRecipe) {
        this.recipe = loadedRecipe;

        icon.sprite = loadedRecipe.producedItems[0].item.icon;
        nameText.SetText(loadedRecipe.producedItems[0].item.itemName);
        amountText.SetText("x" + loadedRecipe.producedItems[0].amount.ToString());
    }

    public void LoadRequireditems(GameObject requiredItemsList) {
        foreach (NumberedItem requiredItem in recipe.requiredItems) {
            SavedSlot tempRequiredItem = new SavedSlot(requiredItem.item, requiredItem.amount);
            GameObject requiredItemSlot = Instantiate(requiredItemPrefab);
            requiredItemSlot.transform.SetParent(requiredItemsList.transform);
            requiredItemSlot.GetComponent<RequiredItemSlot>().SetItem(tempRequiredItem);

            int amountInInventory = InventoryController.instance.inventoryContainer.CountItems(requiredItem.item);
            int amountInHotbar = InventoryController.instance.hotbarContainer.CountItems(requiredItem.item);

            requiredItemSlot.GetComponent<RequiredItemSlot>().SetInputtedAmount(amountInInventory + amountInHotbar);
        }
    }
}
