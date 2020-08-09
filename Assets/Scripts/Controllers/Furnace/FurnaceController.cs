using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceController : MonoBehaviour, IInteractable {
    [SerializeField] private GameObject furnaceInterfacePrefab = default;

    private Container fuelContainer = new Container();
    private Container smeltingContainer = new Container();
    private Container smeltedContainer = new Container();
    private FurnaceUIController furnaceUI;

    private FuelItem fuelItem;
    private FuelItem currentFuelItem;
    private Item smeltingItem;
    private Item currentSmeltingItem;
    private float remainingBurnTime;
    private float progress;

    // Start is called before the first frame update
    void Start()
    {
        fuelContainer.Init(1);
        smeltingContainer.Init(1);
        smeltedContainer.Init(1);

        remainingBurnTime = 0;
        progress = 0;
        fuelItem = fuelContainer.savedSlots[0].item as FuelItem;
        currentFuelItem = fuelItem;
        smeltingItem = smeltingContainer.savedSlots[0].item;
        currentSmeltingItem = smeltingItem;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingBurnTime < 0) {
            remainingBurnTime = 0;
        }
        if (smeltingItem != null && remainingBurnTime == 0 && fuelItem != null && (smeltedContainer.savedSlots[0].item == null || smeltingItem.GetRecipe<Recipe>().item == smeltedContainer.savedSlots[0].item))
        {
            currentFuelItem = fuelItem;
            remainingBurnTime += currentFuelItem.burnTime;
            fuelContainer.SubtractItem(fuelItem, 1);
        }

        if (remainingBurnTime == 0) {
            if (furnaceUI != null) {
                furnaceUI.ShowInactive();
            }
        } else {
            remainingBurnTime -= Time.deltaTime;
            if (furnaceUI != null) {
                furnaceUI.ShowActive();
            }
        }

        if (furnaceUI != null && currentFuelItem != null) {
            furnaceUI.UpdateBurnTimer(remainingBurnTime / currentFuelItem.burnTime);
        }

        if (smeltingItem == null) {
            progress = 0;
            if (furnaceUI != null) {
                furnaceUI.UpdateProgressBar(0);
            }
        }

        if (smeltingItem == null || remainingBurnTime <= 0) {
            return;
        }

        SmeltingRecipe smeltingRecipe = smeltingItem.GetRecipe<SmeltingRecipe>() as SmeltingRecipe;

        if (smeltingRecipe != null && (smeltingRecipe.item == smeltedContainer.savedSlots[0].item || smeltedContainer.savedSlots[0].item == null)) {

            progress += Time.deltaTime;

            if (furnaceUI != null) {
                furnaceUI.UpdateProgressBar(progress / smeltingRecipe.smeltingTime);
            }
            
            int smelted = Mathf.FloorToInt(progress / smeltingRecipe.smeltingTime);

            if (smelted > 0) {
                smeltedContainer.InsertItem(smeltingRecipe.item, smeltingRecipe.amount * smelted);
                smeltingContainer.SubtractItem(smeltingRecipe.recipeItems[0].item, smeltingRecipe.recipeItems[0].amount * smelted);
                progress -= smeltingRecipe.smeltingTime * smelted;
            }
        }
    }

    public void UpdateItems() {
        fuelItem = fuelContainer.savedSlots[0].item as FuelItem;
        smeltingItem = smeltingContainer.savedSlots[0].item;
        if (smeltingItem != null && smeltingItem != currentSmeltingItem) {
            progress = 0;
            furnaceUI.UpdateProgressBar(0);
        }
        currentSmeltingItem = smeltingItem;
    }

    public void OpenInterface()
    {
        InventoryController.instance.OpenInterface(furnaceInterfacePrefab);
        InventoryController.instance.inventoryContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("InventorySlotHolder").gameObject;
        InventoryController.instance.inventoryContainer.Reinit(InventoryController.instance.inventoryContainer.savedSlots);
        InventoryController.instance.hotbarContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("HotbarSlotHolder").gameObject;
        InventoryController.instance.hotbarContainer.Reinit(InventoryController.instance.hotbarContainer.savedSlots);
        fuelContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("FuelSlotHolder").gameObject;
        fuelContainer.Reinit();
        smeltingContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("SmeltingItemSlotHolder").gameObject;
        smeltingContainer.Reinit();
        smeltedContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("SmeltedItemSlotHolder").gameObject;
        smeltedContainer.Reinit();

        furnaceUI = InventoryController.instance._interface.GetComponent<FurnaceUIController>();

        fuelContainer.savedSlots[0].ItemUpdate += () => UpdateItems();
        smeltingContainer.savedSlots[0].ItemUpdate += () => UpdateItems();

        float burnPercent = currentFuelItem != null ? remainingBurnTime / currentFuelItem.burnTime : 0;
        float progressPercent = smeltingItem != null ? progress / (smeltingItem.GetRecipe<SmeltingRecipe>() as SmeltingRecipe).smeltingTime : 0;
        furnaceUI.UpdateBurnTimer(burnPercent);
        furnaceUI.UpdateProgressBar(progressPercent);
    }

    public void LeftClickInteract() {
        
    }

    public void RightClickInteract() {
        OpenInterface();
    }
}
