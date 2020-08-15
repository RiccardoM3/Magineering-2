using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace {

    public Container smeltingContainer = new Container();
    public Container smeltedContainer = new Container();
    public ProgressBar progressBar;

    private Item smeltingItem;
    private Item currentSmeltingItem;
    private float progress;
    private bool isRunning = false;
    public Furnace() {

        smeltingContainer.Init(1);
        smeltedContainer.Init(1);

        smeltingItem = smeltingContainer.savedSlots[0].item;
        currentSmeltingItem = smeltingItem;
        progress = 0;

        TimeTicker.OnTick += delegate (object sender, TimeTicker.OnTickEventArgs e) {
            if (smeltingItem != null && isRunning) {
                SmeltingRecipe smeltingRecipe = smeltingItem.GetRecipe<SmeltingRecipe>() as SmeltingRecipe;

                if (smeltingRecipe != null && (smeltingRecipe.item == smeltedContainer.savedSlots[0].item || smeltedContainer.savedSlots[0].item == null)) {

                    progress += 1;

                    if (progressBar != null) {
                        progressBar.UpdateProgressBar(progress / smeltingRecipe.smeltingTicks);
                    }

                    //if 100% complete
                    if (Mathf.Approximately(progress / smeltingRecipe.smeltingTicks, 1f)) {
                        smeltedContainer.InsertItem(smeltingRecipe.item, smeltingRecipe.amount);
                        smeltingContainer.SubtractItem(smeltingRecipe.recipeItems[0].item, smeltingRecipe.recipeItems[0].amount);
                        ResetProgress();
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

        progressBar = InventoryController.instance._interface.GetComponent<ProgressBar>();
        smeltingContainer.savedSlots[0].ItemUpdate += () => UpdateSmeltingItems();

        float progressPercent = smeltingItem != null ? progress / (smeltingItem.GetRecipe<SmeltingRecipe>() as SmeltingRecipe).smeltingTicks : 0;
        progressBar.UpdateProgressBar(progressPercent);
    }

    public bool CanSmelt() {
        return smeltingItem != null && (smeltedContainer.savedSlots[0].item == null || smeltingItem.GetRecipe<Recipe>().item == smeltedContainer.savedSlots[0].item);
    }

    public void ResetProgress() {
        progress = 0;
        if (progressBar != null) {
            progressBar.UpdateProgressBar(0);
        }
    }

    public void UpdateSmeltingItems() {
        smeltingItem = smeltingContainer.savedSlots[0].item;
        if (smeltingItem != null && smeltingItem != currentSmeltingItem) {
            ResetProgress();
        }
        currentSmeltingItem = smeltingItem;
    }

    public void DoUpdate(bool shouldRun) {

        if (smeltingItem == null) {
            ResetProgress();
        }

        this.isRunning = shouldRun;
    }
}