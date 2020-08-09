using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Resource : MonoBehaviour
{
    public List<itemResource> DroppableItems = new List<itemResource>();

    [Serializable]
    public struct itemResource
    {
        public Item item;
        public int maxAmount;
        public int minAmount;
        public float dropChance;
    }

    public void lootResource()
    {
        foreach (itemResource droppableItem in DroppableItems)
        {
            if ((float)Random.Range(0, 100) / 100 <= droppableItem.dropChance)
            {
                int quantity = Random.Range(droppableItem.minAmount, droppableItem.maxAmount);
                /*Debug.Log("Giving " + quantity.ToString() + " " + droppableItem.item.itemName);*/
                InventoryController.instance.addToInventory(droppableItem.item, quantity);
            } else
            {
                /*Debug.Log("Unlucky! You got nothing.");*/
            }
        }
    }
}
