using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume() {
        InventoryController.instance.CloseActiveInterface();
    }

    public void Controls() {

    }

    public void GraphicsSettings() {

    }

    public void AudioSettings() {

    }

    public void SaveAndGotoMainMenu() {
        SceneManager.LoadScene("Title Scene", LoadSceneMode.Single);
    }

    public void SaveAndExit() {
        Application.Quit();
    }
}
