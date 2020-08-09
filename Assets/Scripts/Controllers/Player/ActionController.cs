using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController
{
    public float range = 10f;

    // Update is called once per frame
    public void checkForActions()
    {
        RaycastHit hit;

        if (Input.GetMouseButtonDown(1))
        {
            var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out hit, range))
            {
                var interactable = hit.transform.GetComponent<IInteractable>();
                if (interactable != null) {
                    interactable.RightClickInteract();
                }
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out hit, range))
            {
                var interactable = hit.transform.GetComponent<IInteractable>();
                if (interactable != null) {
                    interactable.LeftClickInteract();
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InventoryController.instance.CloseActiveInterface();
        }
    }
}
