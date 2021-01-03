using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBenchController : ManualMachine, IInteractable {

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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
    }
}
