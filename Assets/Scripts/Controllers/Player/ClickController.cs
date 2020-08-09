using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickController : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        InventorySlot itemSlot = transform.parent.GetComponent<InventorySlot>();

        //When not holding any item
        if (InventoryController.instance.holdingItem == null || InventoryController.instance.holdingItem.item == null)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (itemSlot.item != null)
                {
                    InventoryController.instance.SetTemporaryHeldItem(itemSlot);
                    itemSlot.ClearSlot();
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (itemSlot.item != null)
                {
                    int fullAmount = itemSlot.amount;
                    int halfAmount = Mathf.CeilToInt((float)fullAmount / 2);
                    itemSlot.SetAmount(halfAmount);
                    InventoryController.instance.SetTemporaryHeldItem(itemSlot);
                    if (fullAmount - halfAmount == 0)
                    {
                        itemSlot.ClearSlot();
                    }
                    else
                    {
                        itemSlot.SetAmount(fullAmount - halfAmount);
                    }
                }
            }
        }
        else //When holding an item
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                itemSlot.PlaceItem(eventData);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {

            }
        }
    }
}
