using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher {
    public CrusherUIController crusherUI;
    public ProgressBar progressBar;
    public int requiredCrushTicks = 160;

    private Container crushingContainer = new Container();
    private Container crushedContainer = new Container();
    private Item crushingItem;
    private Item currentCrushingItem;
    private int progress = 0;
    private bool isRunning = false;

    public Crusher() {
        crushingContainer.Init(1);
        crushedContainer.Init(1);

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

            CrushingRecipe crushingRecipe = crushingItem.GetRecipe<CrushingRecipe>() as CrushingRecipe;

            progress += 1;

            if (progress == requiredCrushTicks) {
                crushedContainer.InsertItem(crushingRecipe.item, crushingRecipe.amount);
                crushingContainer.SubtractItem(crushingRecipe.recipeItems[0].item, crushingRecipe.recipeItems[0].amount);
                progress = 0;
            }

            progressBar.UpdateProgressBar((float)progress / requiredCrushTicks);
        }
    }

    public void ResetProgress() {
        progress = 0;
        if (progressBar != null) {
            progressBar.UpdateProgressBar(0);
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
        crushingContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("CrushingItemSlotHolder").gameObject;
        crushingContainer.Reinit();
        crushedContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("CrushedItemSlotHolder").gameObject;
        crushedContainer.Reinit();
        progressBar = InventoryController.instance._interface.GetComponent<ProgressBar>();

        crushingContainer.savedSlots[0].ItemUpdate += () => UpdateItems();

        if (crushingItem != null) {
            progressBar.UpdateProgressBar((float)progress / requiredCrushTicks);
        }
        else {
            progressBar.UpdateProgressBar(0);
        }
    }
}