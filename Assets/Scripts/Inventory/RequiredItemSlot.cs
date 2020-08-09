using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RequiredItemSlot : MonoBehaviour
{
    public Text itemName;
    public Text amount;
    public Image icon;
    public int inputtedAmount = 0;
    public int requiredAmount;
    public Item item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(SavedSlot savedSlot)
    {
        item = savedSlot.item;
        itemName.text = savedSlot.item.itemName;
        requiredAmount = savedSlot.amount;
        amount.text = inputtedAmount.ToString() + " / " + requiredAmount.ToString();
        icon.sprite = savedSlot.item.icon;
    }

    public void SetInputtedAmount(int amnt)
    {
        inputtedAmount = amnt;
        amount.text = inputtedAmount.ToString() + " / " + requiredAmount.ToString();
    }

    public void SubtractRequiredAmount()
    {
        inputtedAmount -= requiredAmount;
        amount.text = inputtedAmount.ToString() + " / " + requiredAmount.ToString();
    }

    public int RemoveItems()
    {
        int currentAmount = inputtedAmount;
        inputtedAmount = 0;
        amount.text = inputtedAmount.ToString() + " / " + requiredAmount.ToString();
        return currentAmount;
    }

    public bool HasEnoughItems()
    {
        return inputtedAmount >= requiredAmount;

    }
}
