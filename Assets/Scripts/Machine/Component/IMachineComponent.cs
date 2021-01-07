using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IMachineComponent {
    void connectToUI(MachineUIController UIController);
    void tick();
    void update();
}