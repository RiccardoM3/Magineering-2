using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider progressBar;

    public void UpdateProgressBar(float percent) {
        if (percent >= 1) {
            percent = 1;
        }

        progressBar.value = percent * progressBar.maxValue;
    }
}
