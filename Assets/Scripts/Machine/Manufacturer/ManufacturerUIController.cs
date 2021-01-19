using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//will hold functions for mode changing buttons
public class ManufacturerUIController : MachineUIController {

    [SerializeField] private Slider progressBar;

    private Manufacturer linkedManufacturer;

    public Slider GetProgressBar() {
        return this.progressBar;
    }

    public void SetManufacturer(Manufacturer manufacturer) {
        this.linkedManufacturer = manufacturer;
    }

    public void SetMode(int mode) {
        switch (mode) {
            case 0:
                linkedManufacturer.SetMode(Recipe.MachineType.Pressing); 
                break;
            case 1:
                linkedManufacturer.SetMode(Recipe.MachineType.Rolling);
                break;
            case 2:
                linkedManufacturer.SetMode(Recipe.MachineType.Drawing);
                break;
            default:
                linkedManufacturer.SetMode(Recipe.MachineType.Pressing);
                break;
        }
    }

    public void Manufacture() {
        (linkedManufacturer as Manufacturer).Manufacture();
    }
}
