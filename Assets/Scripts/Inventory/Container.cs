using System;
using System.Collections.Generic;
using UnityEngine;

public class Container
{
    public GameObject slotHolder;

    public InventorySlot[] slots;
    public SavedSlot[] savedSlots;

    private int maxSpace;

    //Call when instance of script is made
    public void Init(int space)
    {
        maxSpace = space;
        savedSlots = new SavedSlot[maxSpace];

        for (int i=0; i<space; i++)
        {
            savedSlots[i] = new SavedSlot();
        }
    }

    //Call after GUI has been destroyed and recreated
    public void Reinit(SavedSlot[] loadContainer = null)
    {
        if (loadContainer != null)
        {
            savedSlots = loadContainer;
        }

        slots = slotHolder.GetComponentsInChildren<InventorySlot>();
        InventoryController.instance.SaveContainers += saveItems;
        updateGUI();
    }

    public int InsertItem(Item item, int amount)
    {
        int remaining = amount;
        int index = searchForSlotWithSpace(item, amount);

        //Loop through each item slot that already has the item in it
        while (index >= 0 && remaining > 0)
        {
            int amountInSlot = savedSlots[index].amount;

            //If adding the item would overflow it, max out the stack and subtract from remaining
            if (amountInSlot + remaining > item.stackAmount)
            {
                savedSlots[index].amount = item.stackAmount;
                remaining -= item.stackAmount - amountInSlot;
            }
            //If it wouldn't oveflow it, add the remaining items and set remaining to zero
            else
            {
                savedSlots[index].amount += remaining;
                remaining = 0;
            }

            index = searchForSlotWithSpace(item, amount);
        }

        //Loop through each slot which has nothing in it
        index = SearchForItem();
        while (index >= 0  && remaining > 0)
        {
            //If the item would overflow the slot, max it and subtract from remaining
            if (remaining > item.stackAmount)
            {
                savedSlots[index].setItem(item, item.stackAmount);
                remaining -= item.stackAmount;
            }
            //If the item fits, set the amount and set remaining to zero
            else
            {
                savedSlots[index].setItem(item, remaining);
                remaining = 0;
            }

            index = SearchForItem();
        }

        updateGUI();
        return remaining;
    }

    public int SubtractItem(Item item, int amount)
    {
        int remaining = amount;
        int index = SearchForItem(item);

        //Loop through every slot which has the item
        while (index >= 0 && remaining > 0)
        {
            int slotAmount = savedSlots[index].amount;

            //subtract from every slot until remaining is 0
            if (remaining <= slotAmount)
            {
                slotAmount -= remaining;
                remaining = 0;
            } else
            {
                remaining -= slotAmount;
                slotAmount = 0;
            }

            savedSlots[index].setItem(item, slotAmount);


            index = SearchForItem(item, index);
        }

        updateGUI();
        return remaining;
    }

    private int SearchForItem(Item item = null, int startFrom = 0)
    {
        for (int i = startFrom; i < savedSlots.Length; i++)
        {
            if (savedSlots[i].item == item)
            {
                return i;
            }
        }

        return -1;
    }

    private int searchForSlotWithSpace(Item item, int amount)
    {
        for (int i = 0; i < savedSlots.Length; i++)
        {
            if (savedSlots[i].item == item && savedSlots[i].amount < item.stackAmount)
            {
                return i;
            }
        }

        return -1;
    }

    public int countItems(Item item = null)
    {
        int total = 0;

        for (int i = 0; i < savedSlots.Length; i++)
        {
            if (item == null || savedSlots[i].item == item)
            {
                total += savedSlots[i].amount;
            }
        }

        return total;
    }

    public void updateGUI()
    {
        if (slotHolder != null) {
            for (int i = 0; i < savedSlots.Length; i++) {
                slots[i].SetItem(savedSlots[i].item, savedSlots[i].amount);
            }
        }
    }

    public void saveItems()
    {
        if (slotHolder != null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                savedSlots[i].copyInventorySlot(slots[i]);
            }
        }
    }
}

[Serializable]
public class SavedSlot
{
    public Item item;
    public int amount;

    public delegate void OnItemUpdate();
    public event OnItemUpdate ItemUpdate;

    public void InvokeItemUpdate() {
        ItemUpdate?.Invoke();
    }

    public void UnsubscribeAll() {
        if (ItemUpdate != null)
            foreach (var method in ItemUpdate.GetInvocationList())
                ItemUpdate -= (method as OnItemUpdate);
    }

    public void copyInventorySlot(InventorySlot slot)
    {
        this.item = slot.item;
        this.amount = slot.amount;

        InvokeItemUpdate();
    }
    public void setItem(Item newItem, int amt)
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
