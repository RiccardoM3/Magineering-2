using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricMachine : MonoBehaviour {
    public GameObject UIPrefab;
    public GameObject WireNodeConnection;
    public BaseMachine machine = new BaseMachine();
    public float requiredPowerThisTick;

    private float receivedPowerThisTick;
    private ElectricityUIController electricityUI;
    private WireNode wireNode;

}
