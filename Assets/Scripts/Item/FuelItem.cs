using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fuel Item", menuName = "Inventory/Fuel")]
public class FuelItem : Item {

    public float burnTicks = 160;   //8 seconds
}
