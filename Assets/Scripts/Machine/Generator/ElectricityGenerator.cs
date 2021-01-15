using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElectricityGenerator : Machine, IUsesElectricityComponent {

    [SerializeField] private WireNode outputNode;
    [SerializeField] private float generatedPowerPerTick;

    private ElectricityComponent electricityComponent;

    public override void Init() {
        base.Init();

        this.electricityComponent = new ElectricityComponent(this.outputNode, 0);
    }

    public override void Tick() {
        base.Tick();
        this.electricityComponent.Tick();

        if (this.CanProcess()) {
            outputNode.GetComponent<WireNode>().powerContributionPerTick = -1 * generatedPowerPerTick;
        }
        else {
            outputNode.GetComponent<WireNode>().powerContributionPerTick = 0;
        }
    }

    public ElectricityComponent getElectricityComponent() {
        return this.electricityComponent;
    }
}