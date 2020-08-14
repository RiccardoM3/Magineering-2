using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFurnace : ElectricMachine, IInteractable {
    public Furnace furnace = new Furnace();
    public override void Start() {
        base.Start();
    }

    public override void LateUpdate() {
        base.LateUpdate();

        if (machine.IsEnabled() && HasEnoughPower()) {
            furnace.DoUpdate();
        }
    }

    public void LeftClickInteract() {

    }

    public void RightClickInteract() {
        base.ConnectToUI();
        furnace.ConnectToUI();
    }
}