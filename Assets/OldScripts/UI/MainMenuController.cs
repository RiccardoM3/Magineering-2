using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame() {
        SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
    }

    public void LoadGame() {

    }

    public void Settings() {

    }

    public void Exit() {
        Application.Quit();
    }
}
