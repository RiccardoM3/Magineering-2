using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progressBar;

    public void UpdateProgressBar(float percent) {
        if (percent >= 1) {
            percent = 1;
        }
        progressBar.transform.localScale = new Vector3(percent, 1, 1);
    }
}
