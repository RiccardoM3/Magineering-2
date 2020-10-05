using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace {

    public Container smeltingContainer = new Container();
    public Container smeltedContainer = new Container();
    public ProgressBar progressBar;
    public int requiredSmeltTicks = 160;

    private Item smeltingItem;
    private Item currentSmeltingItem;
    private float progress = 0;
    private bool isRunning = false;

    public Furnace() {

        smeltingContainer.Init(1);
        smeltedContainer.Init(1);

        TimeTicker.OnTick += delegate (object sender, TimeTicker.OnTickEventArgs e) {
            if (smeltingItem != null && isRunning) {
                Recipe smeltingRecipe = smeltingItem.GetRecipe(Recipe.RecipeType.Smelting);

                if (smeltingRecipe != null && (smeltingRecipe.item == smeltedContainer.savedSlots[0].item || smeltedContainer.savedSlots[0].item == null)) {

                    progress += 1;

                    if (progressBar != null) {
                        progressBar.UpdateProgressBar(progress / requiredSmeltTicks);
                    }

                    //if 100% complete
                    if (Mathf.Approximately(progress / requiredSmeltTicks, 1f)) {
                        smeltedContainer.InsertItem(smeltingRecipe.item, smeltingRecipe.amount);
                        smeltingContainer.SubtractItem(smeltingRecipe.recipeItems[0].item, smeltingRecipe.recipeItems[0].amount);
                        ResetProgress();
                    }
                }
            }
        };
    }

    public void ConnectToUI() {
        smeltingContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("MainSection").Find("FurnaceSection").Find("SmeltingItemSlotHolder").gameObject;
        smeltingContainer.Reinit();
        smeltedContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("MainSection").Find("FurnaceSection").Find("SmeltedItemSlotHolder").gameObject;
        smeltedContainer.Reinit();

        progressBar = InventoryController.instance._interface.GetComponent<ProgressBar>();
        smeltingContainer.savedSlots[0].ItemUpdate += () => UpdateSmeltingItems();

        float progressPercent = smeltingItem != null ? progress / requiredSmeltTicks : 0;
        progressBar.UpdateProgressBar(progressPercent);
    }

    public bool CanSmelt() {
        return smeltingItem != null && (smeltedContainer.savedSlots[0].item == null || smeltingItem.GetRecipe(Recipe.RecipeType.Smelting).item == smeltedContainer.savedSlots[0].item);
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