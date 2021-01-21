using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Inventory/Building")]
public class BuildingItem : Item
{
    public GameObject preview;

    public override void Hover()
    {
        base.Hover();
        BuildingManager.instance.StartBuilding(preview);
    }

    public override void Unhover()
    {
        base.Unhover();
        BuildingManager.instance.StopBuilding();
    }
}
