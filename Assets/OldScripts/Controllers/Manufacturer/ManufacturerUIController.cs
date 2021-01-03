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

        Recipe.MachineType manufacturerMode;

        switch (mode) {
            case 0:
                manufacturerMode = Recipe.MachineType.Pressing;
                break;
            case 1:
                manufacturerMode = Recipe.MachineType.Rolling;
                break;
            case 2:
                manufacturerMode = Recipe.MachineType.Drawing;
                break;
            default:
                manufacturerMode = Recipe.MachineType.Pressing;
                break;
        }

        linkedManufacturer.SetMode(manufacturerMode);
    }
}
