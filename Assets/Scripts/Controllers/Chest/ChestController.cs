using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : ManualMachine, IInteractable {
    private Container chestContainer;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        chestContainer = new Container(30, "Sections/MainSection/ChestStorageSection/ChestSlotHolder");
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
        chestContainer.Reinit(chestContainer.savedSlots);
    }
}
