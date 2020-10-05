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

        Manufacturer.Mode manufacturerMode;

        switch (mode) {
            case 0:
                manufacturerMode = Manufacturer.Mode.Plates;
                break;
            case 1:
                manufacturerMode = Manufacturer.Mode.Rods;
                break;
            case 2:
                manufacturerMode = Manufacturer.Mode.Wires;
                break;
            default:
                manufacturerMode = Manufacturer.Mode.Plates;
                break;
        }

        linkedManufacturer.SetMode(manufacturerMode);
    }
}
