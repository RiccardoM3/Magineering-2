using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCrusher : Crusher {

    private int remainingCrushTicks;

    public override void Init() {
        base.Init();

        this.remainingCrushTicks = 0;
    }

    public override void Tick() {
        if (this.CanProcess()) {
            this.progress.addProgress(1 * this.speed);
        }

        if (!HasMatchingRecipeAndFitsInOutputContainer()) {
            this.progress.setProgress(0);
        }

        if (this.progress.getProgress() >= 1) {
            this.remainingCrushTicks = 0;
            this.progress.setProgress(0);
            this.Process();
        }

        if (this.remainingCrushTicks > 0) {
            this.remainingCrushTicks -= 1;
        }
    }

    public void Crush() {
        if (this.HasMatchingRecipeAndFitsInOutputContainer()) {
            this.remainingCrushTicks = 10;  //When the crush button is clicked, the machine is active for 10 ticks.
        }
    }

    public override bool IsActive() {
        return this.isCrushing();
    }

    public override bool CanProcess() {
        return this.isCrushing() && base.CanProcess();
    }

    public bool isCrushing() {
        return this.remainingCrushTicks > 0;
    }
}
