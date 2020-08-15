using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalGeneratorController : MonoBehaviour, IInteractable {

    public GameObject coalGeneratorInterfacePrefab;
    public GameObject outputNode;

    private Container fuelContainer = new Container();
    private CoalGeneratorUIController generatorUI;

    private FuelItem fuelItem;
    private FuelItem currentFuelItem;
    private float remainingBurnTime;
    private float generatedPowerPerTick = 5f;

    // Start is called before the first frame update
    void Start()
    {
        fuelContainer.Init(1);

        remainingBurnTime = 0;
        fuelItem = fuelContainer.savedSlots[0].item as FuelItem;
        currentFuelItem = fuelItem;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingBurnTime < 0) {
            remainingBurnTime = 0;
        }
        if (remainingBurnTime == 0 && fuelItem != null) {
            currentFuelItem = fuelItem;
            remainingBurnTime += currentFuelItem.burnTicks;
            fuelContainer.SubtractItem(fuelItem, 1);
        }

        if (remainingBurnTime == 0) {
            if (generatorUI != null) {
                generatorUI.ShowInactive();
            }
            outputNode.GetComponent<WireNode>().powerContributionPerTick = 0;
        }
        else {
            remainingBurnTime -= Time.deltaTime;
            if (generatorUI != null) {
                generatorUI.ShowActive();
            }
            outputNode.GetComponent<WireNode>().powerContributionPerTick = generatedPowerPerTick;
        }

        if (generatorUI != null && currentFuelItem != null) {
            generatorUI.UpdateBurnTimer(remainingBurnTime / currentFuelItem.burnTicks);
        }
    }

    public void LeftClickInteract() {
        
    }

    public void RightClickInteract() {
        OpenInterface();
    }

    public void OpenInterface() {
        InventoryController.instance.OpenInterface(coalGeneratorInterfacePrefab);
        InventoryController.instance.inventoryContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("InventorySlotHolder").gameObject;
        InventoryController.instance.inventoryContainer.Reinit(InventoryController.instance.inventoryContainer.savedSlots);
        InventoryController.instance.hotbarContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("HotbarSlotHolder").gameObject;
        InventoryController.instance.hotbarContainer.Reinit(InventoryController.instance.hotbarContainer.savedSlots);
        fuelContainer.slotHolder = InventoryController.instance._interface.transform.GetChild(0).Find("FuelSlotHolder").gameObject;
        fuelContainer.Reinit();

        generatorUI = InventoryController.instance._interface.GetComponent<CoalGeneratorUIController>();

        fuelContainer.savedSlots[0].ItemUpdate += () => UpdateItems();

        float burnPercent = currentFuelItem != null ? remainingBurnTime / currentFuelItem.burnTicks : 0;
        generatorUI.UpdateBurnTimer(burnPercent);
    }

    public void UpdateItems() {
        fuelItem = fuelContainer.savedSlots[0].item as FuelItem;
    }

}