using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMachine {

    private bool enabled = true;

    public bool IsEnabled() {
        return enabled;
    }

    public void ConnectInventoryToUI(GameObject UIPrefab) {
        InventoryController.instance.OpenInterface(UIPrefab);
        InventoryController.instance.inventoryContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("ItemStorageSection").Find("InventorySlotHolder").gameObject;
        InventoryController.instance.inventoryContainer.Reinit(InventoryController.instance.inventoryContainer.items);
        InventoryController.instance.hotbarContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("ItemStorageSection").Find("HotbarSlotHolder").gameObject;
        InventoryController.instance.hotbarContainer.Reinit(InventoryController.instance.hotbarContainer.items);
    }
}