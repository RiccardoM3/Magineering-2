using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public delegate void OnItemUpdate();
    public event OnItemUpdate SaveContainers;

    public GameObject _interface;
    public GameObject _hotbar;
    public GameObject inventoryPrefab;
    public GameObject hotbarPrefab;
    public GameObject draggedItemPrefab;
    public GameObject labelPrefab;
    public GameObject menuPrefab;
    public GameObject backgroundCover;
    public Container inventoryContainer;
    public Container hotbarContainer;
    public bool isActive;
    public InventorySlot activeSlot;

    public SavedSlot holdingItem;
    public GameObject draggedItem;
    public GameObject label;

    private bool allowHotbarScrolling;
    private int activeSlotIndex = 0;

    public List<SavedSlot> debugItems;

    private KeyCode[] hotbarKeyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
         KeyCode.Alpha0
     };

    #region Singleton
    public static InventoryController instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found!");
            return;
        }
        instance = this;
    }
    #endregion
    
    public void InvokeItemUpdate()
    {
        SaveContainers?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryContainer = new Container(30, "Sections/ItemStorageSection/InventorySlotHolder");
        hotbarContainer = new Container(10, "Sections/ItemStorageSection/HotbarSlotHolder");
        CreateHotbar();
        setActiveSlot(0);
        isActive = false;
        allowHotbarScrolling = true;
        DebugGiveItems();
    }

    void DebugGiveItems()
    {
        for (int i = 0; i < debugItems.Count; i++)
        {
            AddToInventory(debugItems[i].item, debugItems[i].amount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    public void setActiveSlot(int index)
    {
        if (activeSlot == hotbarContainer.slots[index]) return;
        
        //Restore old focus outline
        if (activeSlot != null)
        {
            activeSlot.transform.Find("Outline").GetComponent<Image>().color = Color.black;
            if (activeSlot.item != null)
            {
                activeSlot.item.Unhover();
            }
        }

        //Add new focus outline
        activeSlotIndex = index;
        activeSlot = hotbarContainer.slots[index];
        activeSlot.transform.Find("Outline").GetComponent<Image>().color = new Color(0f, 165f/255f, 224f/255f);
        if (activeSlot.item != null)
        {
            activeSlot.item.Hover();
        }
    }

    public void OpenInventory()
    {
        OpenInterface(inventoryPrefab);
        inventoryContainer.Reinit();
        hotbarContainer.Reinit();
    }

    public void AllowHotbarScrolling()
    {
        allowHotbarScrolling = true;
    }

    public void PreventHotbarScrolling()
    {
        allowHotbarScrolling = false;
    }

    public void OpenInterface(GameObject interfaceObj) {
        CloseActiveInterface();
        DestroyHotbar();
        _interface = Instantiate(interfaceObj);
        _interface.transform.SetParent(GameObject.Find("Canvas").transform, false);
        GetComponentInChildren<MouseLookController>().FreeCursorLockCamera();
        GetComponent<PlayerMovementController>().LockMovement();
        PreventHotbarScrolling();
        backgroundCover.SetActive(true);
        isActive = true;

        if (activeSlot.item != null)
        {
            activeSlot.item.Unhover();
        }
    }

    public void CloseActiveInterface() {
        if (_interface != null)
        {
            Destroy(_interface);
            DestroyLabel();
            CreateHotbar();
            GetComponentInChildren<MouseLookController>().LockCursorFreeCamera();
            GetComponent<PlayerMovementController>().FreeMovement();
            AllowHotbarScrolling();
            DestroyTemporaryHeldItem();
            UnsubscribeAll();
            backgroundCover.SetActive(false);
            setActiveSlot(activeSlotIndex);
        }
        isActive = false;
    }

    public void UnsubscribeAll() {
        if (SaveContainers != null) {
            foreach (var method in SaveContainers.GetInvocationList()) {
                SaveContainers -= (method as OnItemUpdate);
            }
        }
    }

    public void CreateHotbar() {
         _hotbar = Instantiate(hotbarPrefab, GameObject.Find("Canvas").transform);
        hotbarContainer.slotHolder = _hotbar.transform.GetChild(0).gameObject;
        hotbarContainer.Reinit(hotbarContainer.savedSlots);
    }

    public void DestroyHotbar() {
        Destroy(_hotbar);
        _hotbar = null;
    }

    public void SetTemporaryHeldItem(InventorySlot inventorySlot) {
        holdingItem = new SavedSlot();
        holdingItem.CopyInventorySlot(inventorySlot);

        draggedItem = Instantiate(draggedItemPrefab);
        draggedItem.GetComponentInChildren<Image>().sprite = inventorySlot.icon.sprite;
        draggedItem.GetComponentInChildren<Text>().text = inventorySlot.amount.ToString();
        draggedItem.transform.SetParent(_interface.transform);
    }

    public void SetTemporaryHeldItemAmount(int amt) {
        if (draggedItem != null) {
            holdingItem.amount = amt;
            draggedItem.GetComponentInChildren<Text>().text = amt.ToString();
        }
    }

    public void DestroyTemporaryHeldItem() {
        holdingItem = null;
        Destroy(draggedItem);
    }

    public void AddToInventory(Item item, int amount) {

        ItemNotificationController.instance.UpdateOrCreateNotification(item, amount);

        //Attempt to add to hotbar
        int remaining = hotbarContainer.InsertItem(item, amount);

        //If this fails, add to inventory
        if (remaining > 0)
        {
            remaining = inventoryContainer.InsertItem(item, remaining);
        }

        //If this fails, create a new container and drop it
        if (remaining > 0)
        {
            Debug.Log(remaining + " overflow!");        //TODO
        }
    }

    public int SubtractFromInventory(Item item, int amount) {
        int remaining = amount;
        remaining = inventoryContainer.SubtractItem(item, remaining);
        remaining = hotbarContainer.SubtractItem(item, remaining);
        
        return remaining;
    }

    public void CreateLabel(String name) {
        label = Instantiate(labelPrefab);
        label.GetComponentInChildren<Text>().text = name;
        label.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    public void DestroyLabel() {
        if (label != null) {
            Destroy(label);
        }
    }

    private void HandleInput() {
        //Escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isActive) {
                CloseActiveInterface();
            }
            else {
                OpenInterface(menuPrefab);
            }
        }

        //Inventory Button
        if (Input.GetKeyDown("f")) {
            if (isActive) {
                CloseActiveInterface();
            }
            else {
                OpenInventory();
            }
        }

        //Hotbar active slot buttons
        if (!isActive) {
            for (int i = 0; i < hotbarKeyCodes.Length; i++) {
                if (Input.GetKeyDown(hotbarKeyCodes[i])) {
                    setActiveSlot(i);
                }
            }
        }

        //Hotbar scrolling
        if (allowHotbarScrolling) {
            //Negative to reverse the order of scrolling
            int slotIncrement = -(int)Input.mouseScrollDelta.y;

            //Ignore if not scrolling
            if (slotIncrement != 0) {
                //Handle negative numbers by Shifting up 10, doesnt change the value
                if (slotIncrement < 0) {
                    slotIncrement += 10;
                }
                setActiveSlot((activeSlotIndex + slotIncrement) % 10);
            }
        }

    }
}