using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelUIController : MachineUIController {
    public Sprite firePrefab;
    public Sprite emptyFirePrefab;
    public Image fireSprite;
    public Slider burnTimer;

    void Start() {
        ShowInactive();
    }

    public void ShowActive() {
        fireSprite.sprite = firePrefab;
        fireSprite.color = new Color(1, 1, 1);
    }

    public void ShowInactive() {
        fireSprite.sprite = emptyFirePrefab;
        fireSprite.color = new Color(0.4f, 0.4f, 0.4f);
    }

    
}