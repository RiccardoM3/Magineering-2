using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalGenerator : ElectricityGenerator, IUsesFuelComponent {

    private FuelComponent fuelComponent;

    public override void Init() {
        base.Init();

        this.fuelComponent = new FuelComponent();
    }

    public override void OnUpdate() {
        base.OnUpdate();
        this.fuelComponent.OnUpdate();
    }

    public override void Tick() {
        base.Tick();
        this.fuelComponent.Tick();

        Recipe recipe = this.GetMatchingRecipe();
        if (this.fuelComponent.ShouldConsumeFuel() && recipe != null && this.outputs.CanFitItems(recipe.producedItems)) {
            this.fuelComponent.ConsumeFuel();
        }
    }

    public override bool CanProcess() {
        return this.fuelComponent.HasActiveFuel() && base.CanProcess();
    }

    public override void ConnectMachineToUI() {
        this.fuelComponent.connectToUI(this.UIController);
    }

    public override bool IsActive() {
        return this.fuelComponent.HasActiveFuel();
    }

    public FuelComponent getFuelComponent() {
        return this.fuelComponent;
    }
}