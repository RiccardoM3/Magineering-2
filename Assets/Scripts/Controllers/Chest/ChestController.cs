using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour, IInteractable {
    private Container chestContainer = new Container();
    public GameObject chestInterface;

    // Start is called before the first frame update
    void Start()
    {
        chestContainer.Init(30);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInterface()
    {
        InventoryController.instance.OpenInterface(chestInterface);
        InventoryController.instance.inventoryContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).GetChild(1).gameObject;
        InventoryController.instance.inventoryContainer.Reinit(InventoryController.instance.inventoryContainer.savedSlots);
        InventoryController.instance.hotbarContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).GetChild(2).gameObject;
        InventoryController.instance.hotbarContainer.Reinit(InventoryController.instance.hotbarContainer.savedSlots);
        chestContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).GetChild(0).gameObject;
        chestContainer.Reinit(chestContainer.savedSlots);
    }

    public void LeftClickInteract() {
        
    }

    public void RightClickInteract() {
        OpenInterface();
    }
}
