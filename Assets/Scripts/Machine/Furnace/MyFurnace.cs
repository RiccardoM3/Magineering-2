using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyFurnace : Machine {

    protected Progress progress;

    private int timeToSmelt = 8; //in seconds

    public override void Init() {
        base.Init();

        this.inputs = new Container(1, "Sections/MainSection/FurnaceSection/InputSlotHolder");
        this.outputs = new Container(1, "Sections/MainSection/FurnaceSection/OutputSlotHolder");

        this.progress = new Progress();
        this.progress.setRequiredProgress(this.getRequiredTicksToSmelt());
        this.progress.setProgress(0);
    }

    public override void Tick() {
        base.Tick();

        if (this.CanProcess()) {
            this.progress.addProgress(1 * this.speed);
        } else {
            this.progress.setProgress(0);
        }

        if (this.progress.getProgress() >= 1) {
            this.progress.setProgress(0);
            this.Process();
        }
    }

    public override void ConnectMachineToUI() {
        this.progress.SetProgressSlider((UIController as FurnaceUIController).getProgressBar());
    }

    public int getRequiredTicksToSmelt() {
        return this.timeToSmelt * TimeTicker.ticksPerSecond;
    }
}