using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/NewRecipe")]
public class NewRecipe : ScriptableObject {

    public MachineType machineType;
    public List<NumberedItem> requiredItems;
    public List<NumberedItem> producedItems;

    public enum MachineType {
        Furnace,
        Crusher
    }

    //Checks if a list of NumberedItems is enough to craft this recipe;
    public bool CompareInputs(List<NumberedItem> inputs) {

        foreach (NumberedItem requiredNumberedItem in this.requiredItems) {

            NumberedItem foundInputItem = null;
            foreach(NumberedItem inputNumberedItem in inputs) {
                if (requiredNumberedItem.item.Equals(inputNumberedItem.item)) {
                    foundInputItem = inputNumberedItem;
                    break;
                }
            }

            if (foundInputItem == null) {
                return false;
            } else if (foundInputItem != null && foundInputItem.amount < requiredNumberedItem.amount) {
                return false;
            }
        }

        return true;

        //Works for NumberedItem equality, not Item equality:
        /*var firstNotSecond = inputs.Except(this.requiredItems).ToList();
        var secondNotFirst = this.requiredItems.Except(inputs).ToList();

        return !firstNotSecond.Any() && !secondNotFirst.Any();*/
    }

}