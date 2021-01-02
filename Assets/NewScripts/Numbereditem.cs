using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NumberedItem {
    public Item item;
    public int amount;

    public NumberedItem(Item item, int amount) {
        this.item = item;
        this.amount = amount;
    }

    public override bool Equals(object obj) {

        NumberedItem numberedItem = obj as NumberedItem;
        if (numberedItem == null) {
            return false;
        }

        if (numberedItem.item == this.item && numberedItem.amount == this.amount) {
            return true;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public override string ToString() {
        if (this.amount == 0 || this.item == null) {
            return null;
        }

        return this.item.name + " (" + this.amount + ")";
    }
}