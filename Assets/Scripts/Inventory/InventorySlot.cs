using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public Image icon;
    public Sprite defaultIcon = null;
    public GameObject labelPrefab;
    public int amount;
    public Text amountText;
    public SlotType slotType = SlotType.General;

    private GameObject label;
    private Color oldColor;

    public void Start()
    {
        oldColor = icon.color;
    }

    public void SetItem(Item newItem, int amt) {
        this.item = newItem;
        this.amount = amt;
        if (newItem != null) {
            icon.sprite = item.icon;
            icon.color = new Color(255, 255, 255, 1);
            amountText.text = amount.ToString();
        } else {
            this.amount = 0;
            icon.sprite = null;
            icon.color = new Color(255, 255, 255, 0);
            amountText.text = "";
        }
    }

    //Assumes there is sufficient items
    public void SubtractAmount(int subtractAmt)
    {
        if (this.item == null)
        {
            return;
        }

        if (this.amount > subtractAmt)
        {
            this.SetAmount(this.amount - subtractAmt);
        }
        else
        {
            this.ClearSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        SetAmount(0);
        amountText.enabled = true;

        icon.sprite = defaultIcon;
        if (defaultIcon == null) {
            icon.color = new Color(255, 255, 255, 0);
        } else {
            icon.color = oldColor;
        }

        InventoryController.instance.InvokeItemUpdate();
    }

    public void SetAmount(int amt)
    {
        amount = amt;
        amountText.text = amt > 0 ? amt.ToString() : "" ;

        InventoryController.instance.InvokeItemUpdate();
    }

    public void PlaceItem(PointerEventData eventData)
    {
        SavedSlot holdingItem = InventoryController.instance.holdingItem;

        if (eventData.pointerPress.tag != "InventorySlot" ||
           !holdingItem.item.insertsInto.Contains(this.slotType))
        {
            return;
        }
        
        //If this slot is free, insert it into this slot
        if (this.item == null)
        {
            this.SetItem(holdingItem.item, holdingItem.amount);
            InventoryController.instance.DestroyTemporaryHeldItem();
        }
        //If they are the same item and the amount fits, add them together
        else if (this.item == holdingItem.item && this.amount + holdingItem.amount <= this.item.stackAmount)
        {
            this.amount += holdingItem.amount;
            amountText.text = amount.ToString();
            InventoryController.instance.DestroyTemporaryHeldItem();
        }
        //If they are different items, swap them
        else
        {
            SavedSlot temp = InventoryController.instance.holdingItem;
            InventoryController.instance.DestroyTemporaryHeldItem();
            InventoryController.instance.SetTemporaryHeldItem(this);
            this.SetItem(temp.item, temp.amount);
        }

       InventoryController.instance.InvokeItemUpdate();
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            label = Instantiate(labelPrefab);
            label.GetComponentInChildren<Text>().text = item.itemName;
            label.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(label);
    }
}
public enum SlotType
{
    General,
    None,
    Smeltable,
    Crushable,
    Fuel
}