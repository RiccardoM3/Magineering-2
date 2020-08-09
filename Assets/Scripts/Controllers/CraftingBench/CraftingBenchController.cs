using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBenchController : MonoBehaviour, IInteractable {
    public GameObject craftingBenchInterfacePrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenInterface()
    {
        InventoryController.instance.OpenInterface(craftingBenchInterfacePrefab);
        InventoryController.instance.inventoryContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).GetChild(0).gameObject;
        InventoryController.instance.inventoryContainer.Reinit(InventoryController.instance.inventoryContainer.savedSlots);
        InventoryController.instance.hotbarContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).GetChild(1).gameObject;
        InventoryController.instance.hotbarContainer.Reinit(InventoryController.instance.hotbarContainer.savedSlots);
    }

    public void LeftClickInteract() {
        
    }

    public void RightClickInteract() {
        OpenInterface();
    }
}
