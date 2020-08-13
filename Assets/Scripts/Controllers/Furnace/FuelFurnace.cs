using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelFurnace : FuelMachine, IInteractable {

    public Furnace furnace = new Furnace();

    public override void Start() {
        base.Start();
    }

    public override void Update() {
        base.Update();

        if (furnace.CanSmelt() && NeedsFuel()) {
            ConsumeFuel();
        }

        if (machine.IsEnabled() && HasActiveFuel()) {
            furnace.DoUpdate();
        }
    }

    public void LeftClickInteract() {

    }

    public void RightClickInteract() {
        ConnectToUI();
        furnace.ConnectToUI();
    }
}