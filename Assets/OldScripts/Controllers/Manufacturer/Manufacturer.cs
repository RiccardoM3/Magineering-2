using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manufacturer
{
    public Container inputContainer;
    public Container outputContainer;
    public Progress progressBar;
    public int requiredProgressTicks = 160;

    private Item inputItem;
    private Item currentInputItem;
    private float progress = 0;
    private bool isRunning = false;
    private Recipe.MachineType mode;
    private ManufacturerUIController manufacturerUI;

    public Manufacturer() {

        SetMode(Recipe.MachineType.Pressing);

        inputContainer = new Container(1, "Sections/MainSection/ManufacturerSection/InputSlotHolder");
        outputContainer = new Container(1, "Sections/MainSection/ManufacturerSection/OutputSlotHolder");

        TimeTicker.OnTick += delegate (object sender, TimeTicker.OnTickEventArgs e) {
            if (inputItem != null && isRunning) {

                //TODO needs to be redone
                /*Recipe recipe = inputItem.GetRecipe(this.mode);

                if (recipe != null && (recipe.item == outputContainer.savedSlots[0].item || outputContainer.savedSlots[0].item == null)) {

                    progress += 1;

                    if (progressBar != null) {
                        progressBar.setProgress(progress);
                    }

                    //if 100% complete
                    if (Mathf.Approximately(progress / requiredProgressTicks, 1f)) {
                        outputContainer.InsertItem(recipe.item, recipe.amount);
                        inputContainer.SubtractItem(recipe.recipeItems[0].item, recipe.recipeItems[0].amount);
                        ResetProgress();
                    }
                }*/
            }
        };
    }

    public void ConnectToUI() {
        manufacturerUI = InventoryController.instance._interface.GetComponent<ManufacturerUIController>();
        manufacturerUI.linkedManufacturer = this;

        inputContainer.Reinit();
        outputContainer.Reinit();

        progressBar = InventoryController.instance._interface.GetComponent<Progress>();
        inputContainer.OnItemChange += UpdateItems;

        float progressPercent = inputItem != null ? progress / requiredProgressTicks : 0;
        progressBar.setProgress(inputItem != null ? progress: 0);
    }

    public void ResetProgress() {
        progress = 0;
        if (progressBar != null) {
            progressBar.setProgress(0);
        }
    }

    public void UpdateItems() {
        inputItem = inputContainer.items[0].item;
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
    
    public void SetMode(Recipe.MachineType mode) {
        ResetProgress();
        this.mode = mode;
    }
}
