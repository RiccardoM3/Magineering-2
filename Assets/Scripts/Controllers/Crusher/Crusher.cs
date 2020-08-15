using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher {
    public CrusherUIController crusherUI;
    public ProgressBar progressBar;
    public int requiredCrushTicks = 20;

    private Container crushingContainer = new Container();
    private Container crushedContainer = new Container();
    private Item crushingItem;
    private Item currentCrushingItem;
    private int progress;

    public Crusher() {
        crushingContainer.Init(1);
        crushedContainer.Init(1);
        progress = 0;
    }

    public void UpdateItems() {
        crushingItem = crushingContainer.savedSlots[0].item;
        if (crushingItem == null || currentCrushingItem != crushingItem) {
            progress = 0;
            progressBar.UpdateProgressBar(0);
        }
        currentCrushingItem = crushingItem;
    }

    public void ManualCrush() {
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

    public void ConnectToUI() {
        crushingContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("CrushingItemSlotHolder").gameObject;
        crushingContainer.Reinit();
        crushedContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("CrushedItemSlotHolder").gameObject;
        crushedContainer.Reinit();

        crushingContainer.savedSlots[0].ItemUpdate += () => UpdateItems();

        crusherUI = InventoryController.instance._interface.GetComponent<CrusherUIController>();
        progressBar = InventoryController.instance._interface.GetComponent<ProgressBar>();
        crusherUI.linkedCrusher = this;

        if (crushingItem != null) {
            progressBar.UpdateProgressBar((float)progress / requiredCrushTicks);
        }
        else {
            progressBar.UpdateProgressBar(0);
        }
    }
}