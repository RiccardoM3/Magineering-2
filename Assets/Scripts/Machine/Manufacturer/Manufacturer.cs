using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manufacturer : Machine
{
    [SerializeField] private List<Recipe> pressingRecipes;
    [SerializeField] private List<Recipe> rollingRecipes;
    [SerializeField] private List<Recipe> drawingRecipes;

    private Progress progress;
    private int timeToManufacture = 8; //in seconds
    private Recipe.MachineType mode;
    private int remainingManufactureTicks;
     
    public override void Init() {
        base.Init();

        this.inputs = new Container(1, "Sections/MainSection/ManufacturerSection/InputSlotHolder");
        this.outputs = new Container(1, "Sections/MainSection/ManufacturerSection/OutputSlotHolder");

        this.progress = new Progress();
        this.progress.setRequiredProgress(this.GetRequiredTicksToManufacture());
        this.progress.setProgress(0);

        SetMode(Recipe.MachineType.Pressing);
    }

    public override void Tick() {
        if (this.CanProcess()) {
            this.progress.addProgress(1 * this.speed);
        }

        if (!HasMatchingRecipeAndFitsInOutputContainer()) {
            this.progress.setProgress(0);
        }

        if (this.progress.getProgress() >= 1) {
            this.remainingManufactureTicks = 0;
            this.progress.setProgress(0);
            this.Process();
        }

        if (this.remainingManufactureTicks > 0) {
            this.remainingManufactureTicks -= 1;
        }
    }

    public override void ConnectMachineToUI() {
        (this.UIController as ManufacturerUIController).SetManufacturer(this);
        this.progress.SetProgressSlider((UIController as ManufacturerUIController).GetProgressBar());
    }
    
    public void SetMode(Recipe.MachineType mode) {

        this.mode = mode;
        switch (this.mode) {
            case Recipe.MachineType.Pressing:
                this.recipes = this.pressingRecipes;
                break;
            case Recipe.MachineType.Rolling:
                this.recipes = this.rollingRecipes;
                break;
            case Recipe.MachineType.Drawing:
                this.recipes = this.drawingRecipes;
                break;
        }

        this.progress.setProgress(0);
    }

    public int GetRequiredTicksToManufacture() {
        return this.timeToManufacture * TimeTicker.ticksPerSecond;
    }

    public override bool IsActive() {
        return this.CanProcess();
    }

    public void Manufacture() {
        if (this.HasMatchingRecipeAndFitsInOutputContainer()) {
            this.remainingManufactureTicks = 10;
        }
    }

    public bool isManufacturing() {
        return this.remainingManufactureTicks > 0;
    }

    public override bool CanProcess() {
        return this.isManufacturing() && base.CanProcess();
    }
}
