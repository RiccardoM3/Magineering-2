using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualMachine : MonoBehaviour {
    public GameObject UIPrefab;
    public BaseMachine machine = new BaseMachine();

    public virtual void Start() {

    }

    public virtual void Update() {

    }

    public void ConnectToUI() {
        machine.ConnectInventoryToUI(UIPrefab);
    }
}