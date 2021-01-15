using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFurnace : Furnace, IUsesElectricityComponent {

    [SerializeField] private WireNode wireNode;
    [SerializeField] private float requiredPowerPerTick;

    private ElectricityComponent electricityComponent;

    public override void Init() {
        base.Init(); 
        this.electricityComponent = new ElectricityComponent(wireNode, requiredPowerPerTick);
    }

    public override void OnUpdate() {
        base.OnUpdate();
        this.electricityComponent.OnUpdate();
    }

    public override void Tick() {
        base.Tick();
        this.electricityComponent.Tick();
    }

    public override bool CanProcess() {
        return this.electricityComponent.HasEnoughPower() && base.CanProcess();
    }

    public override void ConnectMachineToUI() {
        base.ConnectMachineToUI();
        this.electricityComponent.connectToUI(this.UIController);
    }

    public override bool IsActive() {
        return this.electricityComponent.HasEnoughPower();
    }

    public ElectricityComponent getElectricityComponent () {
        return this.electricityComponent;
    }
}