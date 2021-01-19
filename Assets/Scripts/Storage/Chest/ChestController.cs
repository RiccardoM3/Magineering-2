using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : InteractibleWithInventory {

    private Container chestContainer;

    // Start is called before the first frame update
    public override void Init() {
        chestContainer = new Container(30, "Sections/MainSection/ChestSlotHolder");
    }

    public override void ConnectToUI() {
        base.ConnectToUI();
        chestContainer.Reinit(chestContainer.items);
    }
}
