using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricMachine : MonoBehaviour {
    public GameObject UIPrefab;
    public GameObject WireNodeConnection;
    public BaseMachine machine = new BaseMachine();
    public float requiredPowerPerSecond;

    private float receivedPower;   //power recieved every deltaTime
    private ElectricityUIController electricityUI;
    private WireNode wireNode;

    public virtual void Start() {
        wireNode = WireNodeConnection.GetComponent<WireNode>();
        wireNode.powerContribution = requiredPowerPerSecond;
    }

    public virtual void LateUpdate() {
        GetPower();
        Debug.Log("Required: " + requiredPowerPerSecond * Time.deltaTime);
        Debug.Log("Received: " + receivedPower);

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
    public void GetPower() {
        receivedPower = wireNode.receivedPower;
    }

    //subtract from the power supply. this is per frame
    public void ConsumePower() {
        receivedPower -= requiredPowerPerSecond * Time.deltaTime;
    }


    public bool HasEnoughPower() {
        return receivedPower / Time.deltaTime >= requiredPowerPerSecond;
    }

    public void ConnectToUI() {
        machine.ConnectInventoryToUI(UIPrefab);

        electricityUI = InventoryController.instance._interface.GetComponent<ElectricityUIController>();
        electricityUI.SetActive(HasEnoughPower());
    }
}
