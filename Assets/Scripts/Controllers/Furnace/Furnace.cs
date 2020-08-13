using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace {

    public Container smeltingContainer = new Container();
    public Container smeltedContainer = new Container();
    public FurnaceUIController furnaceUI;

    private Item smeltingItem;
    private Item currentSmeltingItem;
    private float progress;

    public Furnace() {

        smeltingContainer.Init(1);
        smeltedContainer.Init(1);

        smeltingItem = smeltingContainer.savedSlots[0].item;
        currentSmeltingItem = smeltingItem;
        progress = 0;
    }

    public void ConnectToUI() {
        smeltingContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("SmeltingItemSlotHolder").gameObject;
        smeltingContainer.Reinit();
        smeltedContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("SmeltedItemSlotHolder").gameObject;
        smeltedContainer.Reinit();

        furnaceUI = InventoryController.instance._interface.GetComponent<FurnaceUIController>();
        smeltingContainer.savedSlots[0].ItemUpdate += () => UpdateSmeltingItems();

        float progressPercent = smeltingItem != null ? progress / (smeltingItem.GetRecipe<SmeltingRecipe>() as SmeltingRecipe).smeltingTime : 0;
        furnaceUI.UpdateProgressBar(progressPercent);
    }

    public bool CanSmelt() {
        return smeltingItem != null && (smeltedContainer.savedSlots[0].item == null || smeltingItem.GetRecipe<Recipe>().item == smeltedContainer.savedSlots[0].item);
    }

    public void UpdateSmeltingItems() {
        smeltingItem = smeltingContainer.savedSlots[0].item;
        if (smeltingItem != null && smeltingItem != currentSmeltingItem) {
            progress = 0;
            furnaceUI.UpdateProgressBar(0);
        }
        currentSmeltingItem = smeltingItem;
    }

    public void DoUpdate() {

        if (smeltingItem == null) {
            progress = 0;
            if (furnaceUI != null) {
                furnaceUI.UpdateProgressBar(0);
            }
        }

        if (smeltingItem == null) {
            return;
        }

        SmeltingRecipe smeltingRecipe = smeltingItem.GetRecipe<SmeltingRecipe>() as SmeltingRecipe;

        if (smeltingRecipe != null && (smeltingRecipe.item == smeltedContainer.savedSlots[0].item || smeltedContainer.savedSlots[0].item == null)) {

            progress += Time.deltaTime;

            if (furnaceUI != null) {
                furnaceUI.UpdateProgressBar(progress / smeltingRecipe.smeltingTime);
            }

            int smelted = Mathf.FloorToInt(progress / smeltingRecipe.smeltingTime);

            if (smelted > 0) {
                smeltedContainer.InsertItem(smeltingRecipe.item, smeltingRecipe.amount * smelted);
                smeltingContainer.SubtractItem(smeltingRecipe.recipeItems[0].item, smeltingRecipe.recipeItems[0].amount * smelted);
                progress -= smeltingRecipe.smeltingTime * smelted;
            }
        }
    }
}