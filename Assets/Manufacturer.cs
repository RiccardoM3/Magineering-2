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
    private Recipe.RecipeType mode;
    private ManufacturerUIController manufacturerUI;

    public Manufacturer() {

        SetMode(Recipe.RecipeType.PlateForming);

        inputContainer.Init(1);
        outputContainer.Init(1);

        TimeTicker.OnTick += delegate (object sender, TimeTicker.OnTickEventArgs e) {
            if (inputItem != null && isRunning) {

                Recipe recipe = inputItem.GetRecipe(this.mode);

                if (recipe != null && (recipe.item == outputContainer.savedSlots[0].item || outputContainer.savedSlots[0].item == null)) {

                    progress += 1;

                    if (progressBar != null) {
                        progressBar.UpdateProgressBar(progress / requiredProgressTicks);
                    }

                    //if 100% complete
                    if (Mathf.Approximately(progress / requiredProgressTicks, 1f)) {
                        outputContainer.InsertItem(recipe.item, recipe.amount);
                        inputContainer.SubtractItem(recipe.recipeItems[0].item, recipe.recipeItems[0].amount);
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
        inputContainer.savedSlots[0].ItemUpdate += () => UpdateItems();

        float progressPercent = inputItem != null ? progress / requiredProgressTicks : 0;
        progressBar.UpdateProgressBar(progressPercent);
    }

    /*public bool CanOperate() {
        return inputItem != null && (outputContainer.savedSlots[0].item == null || inputItem.GetRecipe(this.mode).item == outputContainer.savedSlots[0].item);
    }*/

    public void ResetProgress() {
        progress = 0;
        if (progressBar != null) {
            progressBar.UpdateProgressBar(0);
        }
    }

    public void UpdateItems() {
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
    
    public void SetMode(Recipe.RecipeType mode) {
        ResetProgress();
        this.mode = mode;
    }
}
