using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{

    public List<Button> buttons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        if (buttons.Count == 0) {
            return;
        }

        foreach (Button button in buttons) {
            button.onClick.AddListener(() => { UpdateSelection(button); });
        }

        UpdateSelection(buttons[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSelection(Button selectedButton) {

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

        foreach (Button button in buttons) {
            if (button == selectedButton) {
                button.Select();
            }
        }
    }
}
