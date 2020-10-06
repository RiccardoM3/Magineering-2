using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//will hold functions for mode changing buttons
public class ManufacturerUIController : MonoBehaviour
{

    public Manufacturer linkedManufacturer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMode(int mode) {

        Recipe.RecipeType manufacturerMode;

        switch (mode) {
            case 0:
                manufacturerMode = Recipe.RecipeType.PlateForming;
                break;
            case 1:
                manufacturerMode = Recipe.RecipeType.RodForming;
                break;
            case 2:
                manufacturerMode = Recipe.RecipeType.WireForming;
                break;
            default:
                manufacturerMode = Recipe.RecipeType.PlateForming;
                break;
        }

        linkedManufacturer.SetMode(manufacturerMode);
    }
}
