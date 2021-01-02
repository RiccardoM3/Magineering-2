using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelFurnaceUIController : FurnaceUIController
{
    [SerializeField] private Slider burnTimer;

    public Slider getBurnSlider() {
        return this.burnTimer;
    }
}