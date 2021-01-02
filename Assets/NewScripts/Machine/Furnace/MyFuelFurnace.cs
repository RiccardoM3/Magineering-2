using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFuelFurnace : MyFurnace, IUsesFuelAddition
{
    private FuelAddition fuelAddition;

    public override void Init() {
        base.Init();

        this.fuelAddition = new FuelAddition();
    }

    public override void OnUpdate() {
        base.OnUpdate();
        this.fuelAddition.update();
    }

    public override void Tick() {
        base.Tick();
        this.fuelAddition.tick();

        Recipe recipe = this.GetMatchingRecipe();
        if (this.fuelAddition.ShouldConsumeFuel() && recipe != null && this.outputs.CanFitItems(recipe.producedItems)) {
            this.fuelAddition.ConsumeFuel();
        }
    }

    public override bool CanProcess() {
        return this.fuelAddition.HasActiveFuel() && base.CanProcess();
    }

    public override void ConnectMachineToUI() {
        base.ConnectMachineToUI();
        this.fuelAddition.connectToUI(this.UIController);
        
    }

    public override bool IsActive() {
        return this.fuelAddition.HasActiveFuel();
    }

    public FuelAddition getFuelAddition() {
        return this.fuelAddition;
    }
}

public class FuelAddition : IMachineAddition {

    private Container fuelContainer;
    private FuelFurnaceUIController UIController;
    private int remainingBurnTicks;
    private FuelItem fuelItem;
    private FuelItem currentFuelItem;
    private Progress burnTimer;

    public FuelAddition() {
        this.fuelContainer = new Container(1, "Sections/MainSection/FuelSection/InputSlotHolder");
        this.burnTimer = new Progress();
        this.burnTimer.setRequiredProgress(currentFuelItem != null ? currentFuelItem.burnTicks : 0);
    }

    public void connectToUI(MachineUIController UIController) {
        this.UIController = UIController as FuelFurnaceUIController;

        fuelContainer.Reinit();
        fuelContainer.savedSlots[0].ItemUpdate += () => UpdateFuelItems();

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
        fuelItem = fuelContainer.savedSlots[0].item as FuelItem;

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

interface IMachineAddition {
    void connectToUI(MachineUIController UIController);
    void tick();
    void update();
}

interface IUsesFuelAddition {
    FuelAddition getFuelAddition();
}