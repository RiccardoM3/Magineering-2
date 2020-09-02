using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public float range = 10f;

    public void Update()
    {
        //Interactables
        if (!InventoryController.instance.isActive) {
            RaycastHit hit;

            if (Input.GetMouseButtonDown(1)) {
                var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                if (Physics.Raycast(ray, out hit, range)) {
                    var interactable = hit.transform.GetComponent<IInteractable>();
                    if (interactable != null) {
                        interactable.RightClickInteract();
                    }
                }
            }

            if (Input.GetMouseButtonDown(0)) {
                var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                if (Physics.Raycast(ray, out hit, range)) {
                    var interactable = hit.transform.GetComponent<IInteractable>();
                    if (interactable != null) {
                        interactable.LeftClickInteract();
                    }
                }
            }
        }

        //If you right click:
        if (Input.GetMouseButtonDown(1)) {
            if (InventoryController.instance.activeSlot.item != null) {
                InventoryController.instance.activeSlot.item.Use();
            }
        }
    }
}
