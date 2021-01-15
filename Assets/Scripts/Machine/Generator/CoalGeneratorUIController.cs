using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoalGeneratorUIController : MachineUIController, IUsesFuelComponentUI {

    [SerializeField] private Slider burnTimer;

    public Slider getFuelSlider() {
        return this.burnTimer;
    }
}