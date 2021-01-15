using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityComponent : IMachineComponent {

    private FuelFurnaceUIController UIController;
    private WireNode wireNode;
    private float requiredPowerPerTick;
    private float receivedPowerPerTick;

    public ElectricityComponent(WireNode wireNode, float requiredPowerPerTick) {
        this.requiredPowerPerTick = requiredPowerPerTick;
        this.wireNode = wireNode;
        this.wireNode.powerContributionPerTick = this.requiredPowerPerTick;
        this.wireNode.attachedMachine = this;
    }

    public void connectToUI(MachineUIController UIController) {
        this.UIController = UIController as FuelFurnaceUIController;
    }

    public bool HasEnoughPower() {
        return receivedPowerPerTick > requiredPowerPerTick || Mathf.Approximately(receivedPowerPerTick, requiredPowerPerTick); ;
    }

    public void Tick() {
        
    }

    public void OnUpdate() {
        
    }

    //Get all electricity from any connected nodes
    public void ReceivePower(float amount) {
        this.receivedPowerPerTick = amount;
    }
}

public interface IUsesElectricityComponent {
    ElectricityComponent getElectricityComponent();
}