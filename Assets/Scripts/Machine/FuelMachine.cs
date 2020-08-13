using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelMachine : MonoBehaviour {
    public GameObject UIPrefab;
    public BaseMachine machine = new BaseMachine();
    
    private FuelUIController fuelUI;
    private Container fuelContainer = new Container();
    private FuelItem fuelItem;
    private FuelItem currentFuelItem;
    private float remainingBurnTime;

    public bool NeedsFuel() {
        return remainingBurnTime == 0 && fuelItem != null;
    }

    public void ConsumeFuel() {
        currentFuelItem = fuelItem;
        remainingBurnTime += currentFuelItem.burnTime;
        fuelContainer.SubtractItem(fuelItem, 1);
    }

    public bool HasActiveFuel() {
        return remainingBurnTime > 0;
    }

    public void UpdateFuelItems() {
        fuelItem = fuelContainer.savedSlots[0].item as FuelItem;
    }

    public virtual void Start() {
        fuelContainer.Init(1);
        fuelItem = fuelContainer.savedSlots[0].item as FuelItem;
        currentFuelItem = fuelItem;

        remainingBurnTime = 0;
    }

    public virtual void Update() {

        if (remainingBurnTime < 0) {
            remainingBurnTime = 0;
        }

        if (remainingBurnTime == 0) {
            if (fuelUI != null) {
                fuelUI.ShowInactive();
            }
        }
        else {
            remainingBurnTime -= Time.deltaTime;
            if (fuelUI != null) {
                fuelUI.ShowActive();
            }
        }

        if (fuelUI != null && currentFuelItem != null) {
            fuelUI.UpdateBurnTimer(remainingBurnTime / currentFuelItem.burnTime);
        }

    }

    public void ConnectToUI() {
        machine.ConnectInventoryToUI(UIPrefab);
        fuelContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("FuelSlotHolder").gameObject;
        fuelContainer.Reinit();
        fuelContainer.savedSlots[0].ItemUpdate += () => UpdateFuelItems();

        fuelUI = InventoryController.instance._interface.GetComponent<FuelUIController>();

        float burnPercent = currentFuelItem != null ? remainingBurnTime / currentFuelItem.burnTime : 0;
        fuelUI.UpdateBurnTimer(burnPercent);
    }
}