using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RequiredItemSlot : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI amount;
    public Image icon;
    public int inputtedAmount = 0;
    public int requiredAmount;
    public Item item;

    public void SetItem(SavedSlot savedSlot)
    {
        item = savedSlot.item;
        itemName.SetText(savedSlot.item.itemName);
        requiredAmount = savedSlot.amount;
        this.SetAmount(inputtedAmount);
        icon.sprite = savedSlot.item.icon;
    }

    public void SetInputtedAmount(int amnt)
    {
        inputtedAmount = amnt;
        this.SetAmount(inputtedAmount);
    }

    public void SubtractRequiredAmount()
    {
        inputtedAmount -= requiredAmount;
        this.SetAmount(inputtedAmount);
    }

    public bool HasEnoughItems()
    {
        return inputtedAmount >= requiredAmount;
    }

    public void SetAmount(int amt) {
        amount.SetText(amt.ToString() + " / " + requiredAmount.ToString());
        if (this.HasEnoughItems()) {
            amount.color = new Color(0.078f, 0.566f, 0f);
        } else {
            amount.color = new Color(0.766f, 0.14f, 0f);
        }
    }
}
