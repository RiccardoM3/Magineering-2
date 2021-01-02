using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress
{
    private Slider progressSlider;
    private float requiredProgress;
    private float progress;

    public void SetProgressSlider(Slider bar) {
        this.progressSlider = bar;
    }

    public void setRequiredProgress(float requiredProgress) {
        this.requiredProgress = requiredProgress;
    }

    public void addProgress(float amount) {
        this.setProgress(this.progress + amount);
    }

    public void setProgress(float progress) {
        this.progress = progress;

        if (progress > requiredProgress) {
            progress = requiredProgress;
        }

        if (progressSlider != null) {
            progressSlider.value = progress / requiredProgress * progressSlider.maxValue;
        }
    }

    public float getProgress() {
        return this.progress / requiredProgress;
    }
}
