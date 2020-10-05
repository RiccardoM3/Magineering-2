using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manufacturer
{
    public Container inputContainer = new Container();
    public Container outputContainer = new Container();
    public ProgressBar progressBar;
    public int requiredProgressTicks = 160;

    private Item inputItem;
    private Item currentInputItem;
    private float progress = 0;
    private bool isRunning = false;
    private Mode mode;
    private ManufacturerUIController manufacturerUI;

    [Serializable]
    public enum Mode {
        Plates,
        Rods,
        Wires
    }

    public Manufacturer() {

        inputContainer.Init(1);
        outputContainer.Init(1);

        TimeTicker.OnTick += delegate (object sender, TimeTicker.OnTickEventArgs e) {
            if (inputItem != null && isRunning) {
                SmeltingRecipe smeltingRecipe = inputItem.GetRecipe<SmeltingRecipe>() as SmeltingRecipe;

                if (smeltingRecipe != null && (smeltingRecipe.item == outputContainer.savedSlots[0].item || outputContainer.savedSlots[0].item == null)) {

                    progress += 1;

                    if (progressBar != null) {
                        progressBar.UpdateProgressBar(progress / requiredProgressTicks);
                    }

                    //if 100% complete
                    if (Mathf.Approximately(progress / requiredProgressTicks, 1f)) {
                        outputContainer.InsertItem(smeltingRecipe.item, smeltingRecipe.amount);
                        inputContainer.SubtractItem(smeltingRecipe.recipeItems[0].item, smeltingRecipe.recipeItems[0].amount);
                        ResetProgress();
                    }
                }
            }
        };
    }

    public void ConnectToUI() {
        manufacturerUI = InventoryController.instance._interface.GetComponent<ManufacturerUIController>();
        manufacturerUI.linkedManufacturer = this;

        inputContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("MainSection").Find("ManufacturerSection").Find("InputItemSlotHolder").gameObject;
        inputContainer.Reinit();
        outputContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("MainSection").Find("ManufacturerSection").Find("OutputItemSlotHolder").gameObject;
        outputContainer.Reinit();

        progressBar = InventoryController.instance._interface.GetComponent<ProgressBar>();
        inputContainer.savedSlots[0].ItemUpdate += () => UpdateSmeltingItems();

        float progressPercent = inputItem != null ? progress / requiredProgressTicks : 0;
        progressBar.UpdateProgressBar(progressPercent);
    }

    public bool CanSmelt() {
        return inputItem != null && (outputContainer.savedSlots[0].item == null || inputItem.GetRecipe<Recipe>().item == outputContainer.savedSlots[0].item);
    }

    public void ResetProgress() {
        progress = 0;
        if (progressBar != null) {
            progressBar.UpdateProgressBar(0);
        }
    }

    public void UpdateSmeltingItems() {
        inputItem = inputContainer.savedSlots[0].item;
        if (inputItem != null && inputItem != currentInputItem) {
            ResetProgress();
        }
        currentInputItem = inputItem;
    }

    public void DoUpdate(bool shouldRun) {

        if (inputItem == null) {
            ResetProgress();
        }

        this.isRunning = shouldRun;

    }

    public void SetMode(Mode mode) {
        this.mode = mode;
    }
}
