using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceUIController : MonoBehaviour
{
    public Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateProgressBar(float percent) {
        if (percent >= 1) {
            percent = 1;
        }
        progressBar.transform.localScale = new Vector3(percent, 1, 1);
    }
}
