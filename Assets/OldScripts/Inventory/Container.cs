using System;
using System.Collections.Generic;
using UnityEngine;

public class Container {

    public delegate void OnItemChangeEvent();
    public event OnItemChangeEvent OnItemChange;

    public GameObject slotHolder;
    public InventorySlot[] slots;
    public NumberedItem[] items;

    private string slotHolderPath;
    private int maxSpace;

    public Container(int space, string slotHolderPath = null) {

        this.slotHolderPath = slotHolderPath;
        this.maxSpace = space;
        this.items = new NumberedItem[maxSpace];

        for (int i = 0; i < space; i++) {
            this.items[i] = new NumberedItem(null, 0);
        }
    }

    //Call after GUI has been destroyed and recreated
    public void Reinit(NumberedItem[] loadContainer = null) {

        this.FindAndSetSlotHolder();

        if (loadContainer != null) {
            this.items = loadContainer;
        }

        this.slots = slotHolder.GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot inventorySlot in this.slots) {
            inventorySlot.container = this;
        }

        this.OnItemChange += SaveItems;
        UpdateGUI();
    }

    public void InvokeOnItemChange() {
        this.OnItemChange?.Invoke();
    }

    public void UnsubscribeToEvents() {
        if (this.OnItemChange != null) {
            foreach (var method in this.OnItemChange.GetInvocationList()) {
                this.OnItemChange -= (method as OnItemChangeEvent);
            }
        }
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
            int amountInSlot = items[index].amount;

            //If adding the item would overflow it, max out the stack and subtract from remaining
            if (amountInSlot + remaining > item.stackAmount) {
                items[index].amount = item.stackAmount;
                remaining -= item.stackAmount - amountInSlot;
            }
            //If it wouldn't oveflow it, add the remaining items and set remaining to zero
            else {
                items[index].amount += remaining;
                remaining = 0;
            }

            index = SearchForSlotWithSpace(item, amount);
        }

        //Loop through each slot which has nothing in it
        index = SearchForItem();
        while (index >= 0  && remaining > 0) {
            //If the item would overflow the slot, max it and subtract from remaining
            if (remaining > item.stackAmount) {
                items[index].SetItems(item, item.stackAmount);
                remaining -= item.stackAmount;
            }
            //If the item fits, set the amount and set remaining to zero
            else {
                items[index].SetItems(item, remaining);
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
            int slotAmount = items[index].amount;

            //subtract from every slot until remaining is 0
            if (remaining <= slotAmount) {
                slotAmount -= remaining;
                remaining = 0;
            } else {
                remaining -= slotAmount;
                slotAmount = 0;
            }

            items[index].SetItems(item, slotAmount);


            index = SearchForItem(item, index);
        }

        UpdateGUI();
        return remaining;
    }

    private int SearchForItem(Item item = null, int startFrom = 0) {
        for (int i = startFrom; i < items.Length; i++) {
            if (items[i].item == item) {
                return i;
            }
        }

        return -1;
    }

    private int SearchForSlotWithSpace(Item item, int amount) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i].item == item && items[i].amount < item.stackAmount) {
                return i;
            }
        }

        return -1;
    }

    public int CountItems(Item item = null) {
        int total = 0;

        for (int i = 0; i < items.Length; i++) {
            if (item == null || items[i].item == item) {
                total += items[i].amount;
            }
        }

        return total;
    }

    public void UpdateGUI() {
        if (slotHolder != null) {
            for (int i = 0; i < items.Length; i++) {
                slots[i].SetItem(items[i].item, items[i].amount);
            }
        }
    }

    public void SaveItems() {
        if (slotHolder != null) {
            for (int i = 0; i < slots.Length; i++) {
                items[i] = slots[i].ToNumberedItem();
            }
        }
    }

    public List<NumberedItem> ToList() {
        List<NumberedItem> newList = new List<NumberedItem>();
        foreach (NumberedItem numberedItem in this.items) {
            newList.Add(new NumberedItem(numberedItem.item, numberedItem.amount));
        }
        return newList;
    }

    //returns whether the container has enough space for every item in the list
    public bool CanFitItems(List<NumberedItem> itemList) {

        //clone the container
        Container clonedContainer = new Container(this.maxSpace);
        for (int i = 0; i < this.items.Length; i++) {
            clonedContainer.items[i] = new NumberedItem(this.items[i].item, this.items[i].amount);
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