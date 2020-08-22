using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wire Item", menuName = "Inventory/Wire")]
public class WireItem : Item {

    public float maxPower = 100;

    public override void Hover() {
        base.Hover();
        WireManager.instance.StartPlacingWires(this);
    }

    public override void Unhover() {
        base.Unhover();
        WireManager.instance.StopPlacingWires();
    }
}
