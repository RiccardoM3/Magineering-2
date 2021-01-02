using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Machine : MonoBehaviour, IInteractable
{
    [SerializeField] protected float speed = 1.0f;
    [SerializeField] protected List<Recipe> recipes;
    [SerializeField] private GameObject UIPrefab;

    protected MachineUIController UIController;
    protected Container inputs;
    protected Container outputs;

    // Start is called before the first frame update
    void Start() {
        Init();
    }

    // Update is called once per frame
    void Update() {
        OnUpdate();
    }

    //called on Start();
    public virtual void Init() {
        TimeTicker.OnTick += delegate (object sender, TimeTicker.OnTickEventArgs e) {
            this.Tick();
        };
    }

    //called on Update()
    public virtual void OnUpdate() {
        if (UIController != null) {
            if (IsActive()) {
                UIController.showActiveImage();
            }
            else {
                UIController.showInactiveImage();
            }
        }
    }

    //called every tick
    public virtual void Tick() {}

    //returns whether the active image should show or not
    public abstract bool IsActive();

    //returns whether you can process or not this tick
    public virtual bool CanProcess() {
        Recipe recipe = this.GetMatchingRecipe();
        if (recipe == null || !this.outputs.CanFitItems(recipe.producedItems)) {
            return false;
        }

        return true;
    }

    //called when you rightclick it
    public abstract void ConnectMachineToUI();

    //processes inputs to outputs based on recipes
    public virtual void Process() {
        Recipe recipe = this.GetMatchingRecipe();
        if (recipe != null) {

            foreach (NumberedItem requiredNumberedItem in recipe.requiredItems) {
                this.inputs.SubtractItem(requiredNumberedItem.item, requiredNumberedItem.amount);
            }

            foreach (NumberedItem producedNumberedItem in recipe.producedItems) {
                this.outputs.InsertItem(producedNumberedItem.item, producedNumberedItem.amount);
            }
        }
    }

    public void ConnectToUI() {
        this.ConnectInventoryToUI();
        this.ConnectMachineToUI();
    }

    public void ConnectInventoryToUI() {
        InventoryController.instance.OpenInterface(this.UIPrefab);
        InventoryController.instance.inventoryContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("ItemStorageSection").Find("InventorySlotHolder").gameObject;
        InventoryController.instance.inventoryContainer.Reinit(InventoryController.instance.inventoryContainer.savedSlots);
        InventoryController.instance.hotbarContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("ItemStorageSection").Find("HotbarSlotHolder").gameObject;
        InventoryController.instance.hotbarContainer.Reinit(InventoryController.instance.hotbarContainer.savedSlots);

        this.UIController = InventoryController.instance._interface.transform.GetComponentInChildren<MachineUIController>();

        //To handle multiple inputs/outputs, child all InventorySlots to the slotHolder, and order them by heirarchy
        inputs.Reinit();
        outputs.Reinit();
    }

    public void LeftClickInteract() {
        
    }

    public void RightClickInteract() {
        ConnectToUI();
    }

    public Recipe GetMatchingRecipe() {

        foreach (Recipe recipe in this.recipes) {
            if (recipe.CompareInputs(this.inputs.ToList())) {
                return recipe;
            }
        }

        return null;
    }
}