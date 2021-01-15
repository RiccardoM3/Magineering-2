using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceUIController : MachineUIController {

    [SerializeField] private Slider progressBar;

    public Slider getProgressBar() {
        return this.progressBar;
    }
}