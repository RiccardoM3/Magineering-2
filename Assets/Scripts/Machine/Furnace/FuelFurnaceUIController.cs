﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelFurnaceUIController : FurnaceUIController, IUsesFuelComponentUI
{
    [SerializeField] private Slider burnTimer;

    public Slider getFuelSlider() {
        return this.burnTimer;
    }
}