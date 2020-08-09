using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Inventory/Building")]
public class BuildingItem : Item
{
    public GameObject preview;
    private GameObject previewHologram;

    public override void Hover()
    {
        base.Hover();
        previewHologram = Instantiate(preview);
    }

    public override void Unhover()
    {
        base.Unhover();
        if (previewHologram != null)
        {
            Destroy(previewHologram);
            previewHologram = null;
        }
    }
}
