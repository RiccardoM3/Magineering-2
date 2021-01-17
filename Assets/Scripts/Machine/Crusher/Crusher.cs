using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Crusher : Machine {

    protected Progress progress;

    private int timeToCrush = 16; //in seconds
    private Item crushingItem;
    private Item currentCrushingItem;

    public override void Init() {
        base.Init();

        this.inputs = new Container(1, "Sections/MainSection/CrusherSection/InputSlotHolder");
        this.outputs = new Container(1, "Sections/MainSection/CrusherSection/OutputSlotHolder");

        this.progress = new Progress();
        this.progress.setRequiredProgress(this.getRequiredTicksToCrush());
        this.progress.setProgress(0);
    }

    public override void Tick() {
        base.Tick();

        if (this.CanProcess()) {
            this.progress.addProgress(1 * this.speed);
        }
        else {
            this.progress.setProgress(0);
        }

        if (this.progress.getProgress() >= 1) {
            this.progress.setProgress(0);
            this.Process();
        }
    }

    public void UpdateItems() {
        crushingItem = this.inputs.items[0].item;
        if (crushingItem == null || currentCrushingItem != crushingItem) {
            this.progress.setProgress(0);
        }
        currentCrushingItem = crushingItem;
    }

    public override void ConnectMachineToUI() {
        this.progress.SetProgressSlider((UIController as CrusherUIController).GetProgressBar());
        (this.UIController as CrusherUIController).SetLinkedCrusher(this);
    }

    public int getRequiredTicksToCrush() {
        return this.timeToCrush * TimeTicker.ticksPerSecond;
    }
}