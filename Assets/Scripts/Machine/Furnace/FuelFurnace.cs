using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelFurnace : Furnace, IUsesFuelComponent
{
    private FuelComponent fuelAddition;

    public override void Init() {
        base.Init();

        this.fuelAddition = new FuelComponent();
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

    public FuelComponent getFuelAddition() {
        return this.fuelAddition;
    }
}