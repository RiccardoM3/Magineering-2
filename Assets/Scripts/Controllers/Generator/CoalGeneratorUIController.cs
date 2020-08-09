﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoalGeneratorUIController : MonoBehaviour
{
    public Sprite firePrefab;
    public Sprite emptyFirePrefab;
    public Image fireSprite;
    public Image burnTimer;

    // Start is called before the first frame update
    void Start() {
        ShowInactive();
    }

    // Update is called once per frame
    void Update() {

    }

    public void ShowActive() {
        fireSprite.sprite = firePrefab;
        fireSprite.color = new Color(1, 1, 1);
    }

    public void ShowInactive() {
        fireSprite.sprite = emptyFirePrefab;
        fireSprite.color = new Color(0.4f, 0.4f, 0.4f);
    }

    public void UpdateBurnTimer(float percent) {
        if (percent >= 1) {
            percent = 1;
        }
        burnTimer.transform.localScale = new Vector3(1, percent, 1);
    }
}
