using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalGeneratorController : FuelMachine, IInteractable {

    public GameObject outputNode;

    private float generatedPowerPerTick = 5f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (NeedsFuel()) {
            ConsumeFuel();
        }

        if (HasActiveFuel()) {
            outputNode.GetComponent<WireNode>().powerContributionPerTick = generatedPowerPerTick;
        } else {
            outputNode.GetComponent<WireNode>().powerContributionPerTick = 0;
        }
    }

    public void LeftClickInteract() {
        
    }

    public void RightClickInteract() {
        ConnectToUI();
    }
}