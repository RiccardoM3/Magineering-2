using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractibleWithInventory : MonoBehaviour, IInteractable {

    [SerializeField] private GameObject UIPrefab;

    public void Start() {
        Init();
    }

    public void Update() {
        OnUpdate();
    }

    public virtual void Init() {}

    public virtual void OnUpdate() {}

    public virtual void ConnectToUI() {
        ConnectInventoryToUI();
    }

    public void ConnectInventoryToUI() {
        InventoryController.instance.OpenInterface(this.UIPrefab);
        InventoryController.instance.inventoryContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("ItemStorageSection").Find("InventorySlotHolder").gameObject;
        InventoryController.instance.inventoryContainer.Reinit(InventoryController.instance.inventoryContainer.items);
        InventoryController.instance.hotbarContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("ItemStorageSection").Find("HotbarSlotHolder").gameObject;
        InventoryController.instance.hotbarContainer.Reinit(InventoryController.instance.hotbarContainer.items);
    }

    public void LeftClickInteract() {

    }

    public void RightClickInteract() {
        ConnectToUI();
    }
}