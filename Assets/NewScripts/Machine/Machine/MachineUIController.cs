using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MachineUIController : MonoBehaviour {
    [SerializeField] private Sprite activeImage;
    [SerializeField] private Sprite inactiveImage;
    [SerializeField] private Image statusImage;

    public void showActiveImage() {
        statusImage.sprite = activeImage;
    }

    public void showInactiveImage() {
        statusImage.sprite = inactiveImage;
    }

    void Start() {
        init();
    }

    void Update() {
        update();
    }

    //called on Start()
    public virtual void init() { }

    //called on Update()
    public virtual void update() { }
}
