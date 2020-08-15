using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricMachine : MonoBehaviour {
    public GameObject UIPrefab;
    public GameObject WireNodeConnection;
    public BaseMachine machine = new BaseMachine();
    public float requiredPowerPerTick;

    private float receivedPowerPerTick;
    private ElectricityUIController electricityUI;
    private WireNode wireNode;

    public virtual void Start() {
        wireNode = WireNodeConnection.GetComponent<WireNode>();
        wireNode.powerContributionPerTick = requiredPowerPerTick;
        wireNode.attachedMachine = this;
    }

    public virtual void Update() {

        if (HasEnoughPower()) {
            if (electricityUI != null) {
                electricityUI.SetActive(true);
            }
        }
        else {
            if (electricityUI != null) {
                electricityUI.SetActive(false);
            }
        }

    }

    //Get all electricity from any connected nodes
    public void ReceivePower(float amount) {
        receivedPowerPerTick = amount;
    }


    public bool HasEnoughPower() {

        //Debug.Log("received" + receivedPowerPerTick);
        //Debug.Log("required" + requiredPowerPerTick);

        return receivedPowerPerTick > requiredPowerPerTick || Mathf.Approximately(receivedPowerPerTick, requiredPowerPerTick);
    }

    public void ConnectToUI() {
        machine.ConnectInventoryToUI(UIPrefab);

        electricityUI = InventoryController.instance._interface.GetComponent<ElectricityUIController>();
        electricityUI.SetActive(HasEnoughPower());
    }
}
