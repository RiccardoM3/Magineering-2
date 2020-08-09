using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherController : MonoBehaviour, IInteractable {

    [SerializeField] private GameObject crusherInterfacePrefab = default;

    private Container crushingContainer = new Container();
    private Container crushedContainer = new Container();
    private CrusherUIController crusherUI;

    private Item crushingItem;
    private int progress;

    // Start is called before the first frame update
    void Start()
    {
        crushingContainer.Init(1);
        crushedContainer.Init(1);

        progress = 0;
        crushingItem = crushingContainer.savedSlots[0].item;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateItems() {
        crushingItem = crushingContainer.savedSlots[0].item;
        if (crushingItem == null) {
            progress = 0;
            crusherUI.UpdateProgressBar(0);
        }
    }

    public void OpenInterface() {
        InventoryController.instance.OpenInterface(crusherInterfacePrefab);
        InventoryController.instance.inventoryContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("InventorySlotHolder").gameObject;
        InventoryController.instance.inventoryContainer.Reinit(InventoryController.instance.inventoryContainer.savedSlots);
        InventoryController.instance.hotbarContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("HotbarSlotHolder").gameObject;
        InventoryController.instance.hotbarContainer.Reinit(InventoryController.instance.hotbarContainer.savedSlots);
        crushingContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("CrushingItemSlotHolder").gameObject;
        crushingContainer.Reinit();
        crushedContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("CrushedItemSlotHolder").gameObject;
        crushedContainer.Reinit();

        crushingContainer.savedSlots[0].ItemUpdate += () => UpdateItems();

        crusherUI = InventoryController.instance._interface.GetComponent<CrusherUIController>();
        crusherUI.linkedCrusher = this;

        if (crushingItem != null) {
            crusherUI.UpdateProgressBar((float)progress / (crushingItem.GetRecipe<CrushingRecipe>() as CrushingRecipe).amountOfClicks);
        } else {
            crusherUI.UpdateProgressBar(0);
        }
    }

    public void Crush() {
        if (crushingItem != null) {

            CrushingRecipe crushingRecipe = crushingItem.GetRecipe<CrushingRecipe>() as CrushingRecipe;

            progress += 1;
            if (progress == crushingRecipe.amountOfClicks) {
                crushedContainer.InsertItem(crushingRecipe.item, crushingRecipe.amount);
                crushingContainer.SubtractItem(crushingRecipe.recipeItems[0].item, crushingRecipe.recipeItems[0].amount);
                progress = 0;
            }

            crusherUI.UpdateProgressBar((float) progress / crushingRecipe.amountOfClicks);
        }
    }

    public void LeftClickInteract() {
        
    }

    public void RightClickInteract() {
        OpenInterface();
    }
}
