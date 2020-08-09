using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fuel Item", menuName = "Inventory/Wire")]
public class WireItem : Item {

    public float maxPower = 100;
    public GameObject previewController;
    private GameObject insantiatedController;

    public override void Hover() {
        base.Hover();
        insantiatedController = Instantiate(previewController);
        previewController.GetComponent<WireNodePreviewController>().SetWire(this);
    }

    public override void Unhover() {
        base.Unhover();
        if (insantiatedController != null) {
            insantiatedController.GetComponent<WireNodePreviewController>().DestroyAllPreviews();

            insantiatedController = null;
        }
    }
}
