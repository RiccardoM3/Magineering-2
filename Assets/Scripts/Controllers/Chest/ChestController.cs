using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : ManualMachine, IInteractable {
    private Container chestContainer = new Container();

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        chestContainer.Init(30);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void LeftClickInteract() {
        
    }

    public void RightClickInteract() {
        ConnectToUI();
        chestContainer.slotHolder = InventoryController.instance._interface.transform.Find("Sections").Find("ChestStorageSection").Find("ChestSlotHolder").gameObject;
        chestContainer.Reinit(chestContainer.savedSlots);
    }
}
