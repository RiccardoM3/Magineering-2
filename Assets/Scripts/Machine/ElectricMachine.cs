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
        wireNode.powerContribution = -1 * requiredPowerPerSecond;
    }

    public virtual void LateUpdate() {
        GetPower();
    }

    //Get all electricity from any connected nodes
    public void GetPower() {
        receivedPower = wireNode.receivedPower;
    }

    //subtract from the power supply. this is per frame
    public void ConsumePower() {
        receivedPower -= requiredPowerPerSecond / Time.deltaTime;
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
