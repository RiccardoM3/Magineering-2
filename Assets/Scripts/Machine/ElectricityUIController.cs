using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricityUIController : MonoBehaviour
{
    public Sprite electricityPrefab;
    public Sprite emptyElectricityPrefab;
    public Image fireSprite;

    void Start() {
        SetActive(false);
    }

    public void SetActive(bool state) {
        if (state) {
            fireSprite.sprite = electricityPrefab;
            fireSprite.color = new Color(1, 1, 1);
        } else {
            fireSprite.sprite = emptyElectricityPrefab;
            fireSprite.color = new Color(0.4f, 0.4f, 0.4f);
        }
    }
}
