﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricCrusher : ElectricMachine, IInteractable {
    public Crusher crusher = new Crusher();

    public void Start()
    {
        //base.Start();
    }

    public void Update()
    {
        //base.Update();
        //crusher.DoUpdate(machine.IsEnabled() && HasEnoughPower());
    }

    public void LeftClickInteract() {

    }

    public void RightClickInteract() {
        //base.ConnectToUI();
        //crusher.ConnectToUI();
    }
}
