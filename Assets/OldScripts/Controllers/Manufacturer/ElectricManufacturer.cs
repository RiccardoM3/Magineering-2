﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricManufacturer : ElectricMachine, IInteractable
{
    public Manufacturer manufacturer = new Manufacturer();
    public void Start() {
        //base.Start();
    }

    public void Update() {
        //base.Update();
        //manufacturer.DoUpdate(machine.IsEnabled() && HasEnoughPower());
    }

    public void LeftClickInteract() {

    }

    public void RightClickInteract() {
        //base.ConnectToUI();
        //manufacturer.ConnectToUI();
    }
}
