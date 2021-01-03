using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricityUIController : MonoBehaviour
{
    public Sprite electricityPrefab;
    public Sprite emptyElectricityPrefab;
    public Image electricitySprite;

    void Start() {
        SetActive(false);
    }

    public void SetActive(bool state) {
        if (state) {
            electricitySprite.sprite = electricityPrefab;
        } else {
            electricitySprite.sprite = emptyElectricityPrefab;
        }
    }
}
