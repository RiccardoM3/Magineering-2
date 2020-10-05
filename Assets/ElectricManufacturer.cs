using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricManufacturer : ElectricMachine, IInteractable
{
    public Manufacturer manufacturer = new Manufacturer();
    public override void Start() {
        base.Start();
    }

    public override void Update() {
        base.Update();
        manufacturer.DoUpdate(machine.IsEnabled() && HasEnoughPower());
    }

    public void LeftClickInteract() {

    }

    public void RightClickInteract() {
        base.ConnectToUI();
        manufacturer.ConnectToUI();
    }
}
