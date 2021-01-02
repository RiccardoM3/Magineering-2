using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher {
    public CrusherUIController crusherUI;
    public Progress progressBar;
    public int requiredCrushTicks = 160;

    private Container crushingContainer;
    private Container crushedContainer;
    private Item crushingItem;
    private Item currentCrushingItem;
    private int progress = 0;
    private bool isRunning = false;

    public Crusher() {
        crushingContainer = new Container(1, "Sections/MainSection/CrusherSection/CrushingItemSlotHolder");
        crushedContainer = new Container(1, "Sections/MainSection/CrusherSection/CrushedItemSlotHolder");

        TimeTicker.OnTick += delegate (object sender, TimeTicker.OnTickEventArgs e) {
            if (isRunning) {
                CrushTick();
            }
        };
    }

    public void UpdateItems() {
        crushingItem = crushingContainer.savedSlots[0].item;
        if (crushingItem == null || currentCrushingItem != crushingItem) {
            ResetProgress();
        }
        currentCrushingItem = crushingItem;
    }

    public void CrushTick() {
        if (crushingItem != null) {

            Recipe crushingRecipe = crushingItem.GetRecipe(Recipe.RecipeType.Crushing);

            progress += 1;

            if (progress == requiredCrushTicks) {
                crushedContainer.InsertItem(crushingRecipe.item, crushingRecipe.amount);
                crushingContainer.SubtractItem(crushingRecipe.recipeItems[0].item, crushingRecipe.recipeItems[0].amount);
                progress = 0;
            }

            progressBar.setProgress(progress);
        }
    }

    public void ResetProgress() {
        progress = 0;
        if (progressBar != null) {
            progressBar.setProgress(0);
        }
    }

    public void DoUpdate(bool shouldRun) {

        if (crushingItem == null) {
            ResetProgress();
        }

        this.isRunning = shouldRun;
    }

    public void ConnectToManualUI() {
        crusherUI = InventoryController.instance._interface.GetComponent<CrusherUIController>();
        crusherUI.linkedCrusher = this;
    }

    public void ConnectToUI() {
        crushingContainer.Reinit();
        crushedContainer.Reinit();
        progressBar = InventoryController.instance._interface.GetComponent<Progress>();

        crushingContainer.savedSlots[0].ItemUpdate += () => UpdateItems();

        if (crushingItem != null) {
            progressBar.setProgress(progress);
        }
        else {
            progressBar.setProgress(0);
        }
    }
}