using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrusherUIController : MachineUIController {

    [SerializeField] private Slider progressBar;

    private Crusher linkedCrusher;

    public Slider GetProgressBar() {
        return this.progressBar;
    }

    public void SetLinkedCrusher(Crusher crusher) {
        this.linkedCrusher = crusher;
    }

    public void Crush() {
        (linkedCrusher as ManualCrusher).Crush();
    }
}
