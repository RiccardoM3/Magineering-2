using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFurnace : ElectricMachine, IInteractable {
    public Furnace furnace = new Furnace();
    public override void Start() {
        base.Start();
    }

    public override void Update() {
        base.Update();
    }

    public void LeftClickInteract() {
        
    }

    public void RightClickInteract() {
        base.OpenInterface();   

    }
}

public class ElectricMachine : MonoBehaviour {

    public BaseMachine machine = new BaseMachine();
    public virtual void Start() {

    }

    public virtual void Update() {

    }

    public void OpenInterface() {
        //machine.ConnectInventoryToUI();
        //TODO connect electric UI elements
    }
}
