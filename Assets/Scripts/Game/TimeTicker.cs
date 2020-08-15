using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTicker : MonoBehaviour
{
    public class OnTickEventArgs : EventArgs {
        public int tick;
    }

    public static event EventHandler<OnTickEventArgs> OnTick;
    public static float lengthOfTick = 0.05f;

    private int tick;
    private float tickTimer;
    

    // Start is called before the first frame update
    void Start()
    {
        tick = 0;
        tickTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= lengthOfTick) {
            tickTimer %= lengthOfTick;
            tick++;
            OnTick?.Invoke(this, new OnTickEventArgs { tick = tick });
        }
    }

    /*void example() {
        TimeTickManager.OnTick += delegate (object sender, TimeTickManager.OnTickEventArgs e) {
            Debug.Log("tick: " + e.tick);
        };
    }*/
}


