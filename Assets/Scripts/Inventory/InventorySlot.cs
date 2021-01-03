using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Container container;
    public Item item;
    public Image icon;
    public Sprite defaultIcon = null;
    public int amount;
    public Text amountText;
    public SlotType slotType = SlotType.General;

    private Color oldColor;
    private Color emptyColor;
    private Color unemptyColor;
    private Image backgroundImage;

    public void Awake() {
        oldColor = icon.color;
        emptyColor = new Color(0.75f, 0.7566214f, 1f, 0.3137255f);
        unemptyColor = new Color(0.75f, 0.7566214f, 1f, 1f);
        backgroundImage = GetComponent<Image>();
    }

    public void SetItem(Item newItem, int amt) {

        Item oldItem = this.item;

        this.item = newItem;
        this.amount = amt;
        if (newItem != null) {
            icon.sprite = item.icon;
            icon.color = new Color(255, 255, 255, 1);
            amountText.text = amount.ToString();
            backgroundImage.color = unemptyColor;
        } else {
            this.amount = 0;
            icon.sprite = null;
            icon.color = new Color(255, 255, 255, 0);
            amountText.text = "";
            backgroundImage.color = emptyColor;
        }

        if (newItem != null && oldItem != newItem && InventoryController.instance.activeSlot == this) {
            this.item.Hover();
        }

    }

    //Assumes there is sufficient items
    public void SubtractAmount(int subtractAmt)
    {
        if (this.item == null) {
            return;
        }

        if (this.amount > subtractAmt) {
            this.SetAmount(this.amount - subtractAmt);
        } else {
            this.ClearSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        SetAmount(0);
        amountText.enabled = true;

        //change icon colour
        icon.sprite = defaultIcon;
        if (defaultIcon == null) {
            icon.color = new Color(255, 255, 255, 0);
        } else {
            icon.color = oldColor;
        }

        //change background colour
        backgroundImage.color = emptyColor;

        this.container.InvokeOnItemChange();
    }

    public void SetAmount(int amt)
    {
        amount = amt;
        amountText.text = amt > 0 ? amt.ToString() : "" ;

        this.container.InvokeOnItemChange();
    }

    public void PlaceItem(PointerEventData eventData)
    {
        NumberedItem holdingItem = InventoryController.instance.holdingItem;

        if (eventData.pointerPress.tag != "InventorySlot" || !holdingItem.item.insertsInto.Contains(this.slotType)) {
            return;
        }

        InventoryController.instance.CreateLabel(holdingItem.item.name);

        //If this slot is free, insert it into this slot
        if (this.item == null)
        {
            this.SetItem(holdingItem.item, holdingItem.amount);
            InventoryController.instance.DestroyTemporaryHeldItem();
        }
        //If they are the same item
        else if (this.item == holdingItem.item)
        {
            //If they are the same item and the amount fits into the spot, add the amounts together
            if (this.amount + holdingItem.amount <= this.item.stackAmount) {
                this.SetAmount(this.amount + holdingItem.amount);
                InventoryController.instance.DestroyTemporaryHeldItem();
            }
            //If they are the same item and the amount doesn't fit, add as much as possible
            else {
                int amountToFill = this.item.stackAmount - this.amount;
                this.SetAmount(this.amount + amountToFill);
                InventoryController.instance.SetTemporaryHeldItemAmount(holdingItem.amount - amountToFill);
            }
            
        }
        //If they are different items, swap them
        else
        {
            NumberedItem temp = InventoryController.instance.holdingItem;
            InventoryController.instance.DestroyTemporaryHeldItem();
            InventoryController.instance.SetTemporaryHeldItem(this);
            this.SetItem(temp.item, temp.amount);
        }

       this.container.InvokeOnItemChange();
        
    }

    public NumberedItem ToNumberedItem() {
        return new NumberedItem(this.item, this.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && InventoryController.instance.holdingItem == null) {
            InventoryController.instance.CreateLabel(item.itemName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryController.instance.DestroyLabel();
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