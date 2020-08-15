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

        TimeTicker.OnTick += delegate (object sender, TimeTicker.OnTickEventArgs e) {
            if (smeltingItem != null) {
                SmeltingRecipe smeltingRecipe = smeltingItem.GetRecipe<SmeltingRecipe>() as SmeltingRecipe;

                if (smeltingRecipe != null && (smeltingRecipe.item == smeltedContainer.savedSlots[0].item || smeltedContainer.savedSlots[0].item == null)) {

                    progress += 1;

                    if (furnaceUI != null) {
                        furnaceUI.UpdateProgressBar(progress / smeltingRecipe.smeltingTicks);
                    }

                    //100% complete
                    if (Mathf.Approximately(progress / smeltingRecipe.smeltingTicks, 1f)) {
                        smeltedContainer.InsertItem(smeltingRecipe.item, smeltingRecipe.amount);
                        smeltingContainer.SubtractItem(smeltingRecipe.recipeItems[0].item, smeltingRecipe.recipeItems[0].amount);
                        progress -= smeltingRecipe.smeltingTicks;
                        furnaceUI.UpdateProgressBar(progress / smeltingRecipe.smeltingTicks);
                    }
                }
            }
        };
    }

    public void ConnectToUI() {
        smeltingContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("SmeltingItemSlotHolder").gameObject;
        smeltingContainer.Reinit();
        smeltedContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("SmeltedItemSlotHolder").gameObject;
        smeltedContainer.Reinit();

        furnaceUI = InventoryController.instance._interface.GetComponent<FurnaceUIController>();
        smeltingContainer.savedSlots[0].ItemUpdate += () => UpdateSmeltingItems();

        float progressPercent = smeltingItem != null ? progress / (smeltingItem.GetRecipe<SmeltingRecipe>() as SmeltingRecipe).smeltingTicks : 0;
        furnaceUI.UpdateProgressBar(progressPercent);
    }

    public bool CanSmelt() {
        return smeltingItem != null && (smeltedContainer.savedSlots[0].item == null || smeltingItem.GetRecipe<Recipe>().item == smeltedContainer.savedSlots[0].item);
    }

    public void ResetProgress() {
        progress = 0;
        if (furnaceUI != null) {
            furnaceUI.UpdateProgressBar(0);
        }
    }

    public void UpdateSmeltingItems() {
        smeltingItem = smeltingContainer.savedSlots[0].item;
        if (smeltingItem != null && smeltingItem != currentSmeltingItem) {
            ResetProgress();
        }
        currentSmeltingItem = smeltingItem;
    }

    public void DoUpdate() {

        if (smeltingItem == null) {
            ResetProgress();
        }
    }
}