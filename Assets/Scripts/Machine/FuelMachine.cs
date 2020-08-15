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
    private float remainingBurnTicks;

    public bool NeedsFuel() {
        return remainingBurnTicks == 0 && fuelItem != null;
    }

    public void ConsumeFuel() {
        currentFuelItem = fuelItem;
        remainingBurnTicks += currentFuelItem.burnTicks;
        fuelContainer.SubtractItem(fuelItem, 1);
    }

    public bool HasActiveFuel() {
        return remainingBurnTicks > 0;
    }

    public void UpdateFuelItems() {
        fuelItem = fuelContainer.savedSlots[0].item as FuelItem;
    }

    public virtual void Start() {
        fuelContainer.Init(1);
        fuelItem = fuelContainer.savedSlots[0].item as FuelItem;
        currentFuelItem = fuelItem;

        remainingBurnTicks = 0;

        TimeTicker.OnTick += delegate (object sender, TimeTicker.OnTickEventArgs e) {

            if (remainingBurnTicks > 0) {
                remainingBurnTicks -= 1;
            }
        };
    }

    public virtual void Update() {

        if (remainingBurnTicks == 0) {
            if (fuelUI != null) {
                fuelUI.ShowInactive();
            }
        } else {
            if (fuelUI != null) {
                fuelUI.ShowActive();
            }
        }

        if (fuelUI != null && currentFuelItem != null) {
            fuelUI.UpdateBurnTimer(remainingBurnTicks / currentFuelItem.burnTicks);
        }

    }

    public void ConnectToUI() {
        machine.ConnectInventoryToUI(UIPrefab);
        fuelContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("FuelSlotHolder").gameObject;
        fuelContainer.Reinit();
        fuelContainer.savedSlots[0].ItemUpdate += () => UpdateFuelItems();

        fuelUI = InventoryController.instance._interface.GetComponent<FuelUIController>();

        float burnPercent = currentFuelItem != null ? remainingBurnTicks / currentFuelItem.burnTicks : 0;
        fuelUI.UpdateBurnTimer(burnPercent);
    }
}