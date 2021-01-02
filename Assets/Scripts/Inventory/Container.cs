using System;
using System.Collections.Generic;
using UnityEngine;

public class Container {
    public GameObject slotHolder;
    public InventorySlot[] slots;
    public SavedSlot[] savedSlots;

    private string slotHolderPath;
    private int maxSpace;

    public Container(int space, string slotHolderPath = null) {

        this.slotHolderPath = slotHolderPath;
        this.maxSpace = space;
        this.savedSlots = new SavedSlot[maxSpace];

        for (int i = 0; i < space; i++) {
            this.savedSlots[i] = new SavedSlot();
        }
    }

    //Call after GUI has been destroyed and recreated
    public void Reinit(SavedSlot[] loadContainer = null) {

        this.FindAndSetSlotHolder();

        if (loadContainer != null) {
            this.savedSlots = loadContainer;
        }

        this.slots = slotHolder.GetComponentsInChildren<InventorySlot>();
        InventoryController.instance.SaveContainers += SaveItems;
        UpdateGUI();
    }

    public void FindAndSetSlotHolder() {

        if (this.slotHolderPath == null || InventoryController.instance._interface == null) {
            return;
        }

        this.slotHolder = InventoryController.instance._interface.transform.Find(this.slotHolderPath).gameObject;
    }

    public int InsertItem(Item item, int amount) {
        int remaining = amount;
        int index = SearchForSlotWithSpace(item, amount);

        //Loop through each item slot that already has the item in it
        while (index >= 0 && remaining > 0)
        {
            int amountInSlot = savedSlots[index].amount;

            //If adding the item would overflow it, max out the stack and subtract from remaining
            if (amountInSlot + remaining > item.stackAmount) {
                savedSlots[index].amount = item.stackAmount;
                remaining -= item.stackAmount - amountInSlot;
            }
            //If it wouldn't oveflow it, add the remaining items and set remaining to zero
            else {
                savedSlots[index].amount += remaining;
                remaining = 0;
            }

            index = SearchForSlotWithSpace(item, amount);
        }

        //Loop through each slot which has nothing in it
        index = SearchForItem();
        while (index >= 0  && remaining > 0) {
            //If the item would overflow the slot, max it and subtract from remaining
            if (remaining > item.stackAmount) {
                savedSlots[index].SetItem(item, item.stackAmount);
                remaining -= item.stackAmount;
            }
            //If the item fits, set the amount and set remaining to zero
            else {
                savedSlots[index].SetItem(item, remaining);
                remaining = 0;
            }

            index = SearchForItem();
        }

        UpdateGUI();
        return remaining;
    }

    public int SubtractItem(Item item, int amount) {
        int remaining = amount;
        int index = SearchForItem(item);

        //Loop through every slot which has the item
        while (index >= 0 && remaining > 0) {
            int slotAmount = savedSlots[index].amount;

            //subtract from every slot until remaining is 0
            if (remaining <= slotAmount) {
                slotAmount -= remaining;
                remaining = 0;
            } else {
                remaining -= slotAmount;
                slotAmount = 0;
            }

            savedSlots[index].SetItem(item, slotAmount);


            index = SearchForItem(item, index);
        }

        UpdateGUI();
        return remaining;
    }

    private int SearchForItem(Item item = null, int startFrom = 0) {
        for (int i = startFrom; i < savedSlots.Length; i++) {
            if (savedSlots[i].item == item) {
                return i;
            }
        }

        return -1;
    }

    private int SearchForSlotWithSpace(Item item, int amount) {
        for (int i = 0; i < savedSlots.Length; i++) {
            if (savedSlots[i].item == item && savedSlots[i].amount < item.stackAmount) {
                return i;
            }
        }

        return -1;
    }

    public int CountItems(Item item = null) {
        int total = 0;

        for (int i = 0; i < savedSlots.Length; i++) {
            if (item == null || savedSlots[i].item == item) {
                total += savedSlots[i].amount;
            }
        }

        return total;
    }

    public void UpdateGUI() {
        if (slotHolder != null) {
            for (int i = 0; i < savedSlots.Length; i++) {
                slots[i].SetItem(savedSlots[i].item, savedSlots[i].amount);
            }
        }
    }

    public void SaveItems() {
        if (slotHolder != null) {
            for (int i = 0; i < slots.Length; i++) {
                savedSlots[i].CopyInventorySlot(slots[i]);
            }
        }
    }

    public List<NumberedItem> ToList() {
        List<NumberedItem> newList = new List<NumberedItem>();
        foreach (SavedSlot savedSlot in this.savedSlots) {
            newList.Add(new NumberedItem(savedSlot.item, savedSlot.amount));
        }
        return newList;
    }

    //returns whether the container has enough space for every item in the list
    public bool CanFitItems(List<NumberedItem> itemList) {

        //clone the container
        Container clonedContainer = new Container(this.maxSpace);
        for (int i = 0; i < this.savedSlots.Length; i++) {
            clonedContainer.savedSlots[i] = new SavedSlot(this.savedSlots[i].item, this.savedSlots[i].amount);
        }

        //see if you can successfully add each item
        foreach (NumberedItem item in itemList) {
            if (clonedContainer.InsertItem(item.item, item.amount) != 0) {
                return false;
            }
        }
        
        return true;
    }
}

[Serializable]
public class SavedSlot
{
    public Item item;
    public int amount;

    public delegate void OnItemUpdate();
    public event OnItemUpdate ItemUpdate;

    public SavedSlot() {

    }

    public SavedSlot(Item item, int amount) {
        this.item = item;
        this.amount = amount;
    }

    public void InvokeItemUpdate() {
        ItemUpdate?.Invoke();
    }

    public void UnsubscribeAll() {
        if (ItemUpdate != null)
            foreach (var method in ItemUpdate.GetInvocationList())
                ItemUpdate -= (method as OnItemUpdate);
    }

    public void CopyInventorySlot(InventorySlot slot)
    {
        this.item = slot.item;
        this.amount = slot.amount;

        InvokeItemUpdate();
    }
    public void SetItem(Item newItem, int amt)
    {
        if (amt == 0) {
            this.item = null;
            this.amount = 0;
        } else {
            this.item = newItem;
            this.amount = amt;
        }

        InvokeItemUpdate();
    }
}
