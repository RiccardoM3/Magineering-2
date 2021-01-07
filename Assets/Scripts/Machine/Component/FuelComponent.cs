using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelComponent : IMachineComponent {

    private Container fuelContainer;
    private FuelFurnaceUIController UIController;
    private int remainingBurnTicks;
    private FuelItem fuelItem;
    private FuelItem currentFuelItem;
    private Progress burnTimer;

    public FuelComponent() {
        this.fuelContainer = new Container(1, "Sections/MainSection/FuelSection/InputSlotHolder");
        this.burnTimer = new Progress();
        this.burnTimer.setRequiredProgress(currentFuelItem != null ? currentFuelItem.burnTicks : 0);
    }

    public void connectToUI(MachineUIController UIController) {
        this.UIController = UIController as FuelFurnaceUIController;

        fuelContainer.Reinit();
        fuelContainer.OnItemChange += UpdateFuelItems;

        this.burnTimer.SetProgressSlider(this.UIController.getBurnSlider());
        this.burnTimer.setProgress(remainingBurnTicks);
    }

    public void ConsumeFuel() {
        currentFuelItem = fuelItem;
        remainingBurnTicks += currentFuelItem.burnTicks;
        fuelContainer.SubtractItem(fuelItem, 1);
    }

    public bool HasActiveFuel() {
        return remainingBurnTicks > 0;
    }

    public bool ShouldConsumeFuel() {
        return remainingBurnTicks == 0 && this.fuelItem != null;
    }

    public void UpdateFuelItems() {
        fuelItem = fuelContainer.items[0].item as FuelItem;

        if (this.UIController != null) {
            this.burnTimer.setRequiredProgress(currentFuelItem != null ? currentFuelItem.burnTicks : 0);
        }
    }

    public void tick() {
        if (HasActiveFuel()) {
            remainingBurnTicks -= 1;
        }
    }

    public void update() {
        if (UIController != null) {
            this.burnTimer.setProgress(remainingBurnTicks);
        }
    }
}

interface IUsesFuelComponent {
    FuelComponent getFuelAddition();
}