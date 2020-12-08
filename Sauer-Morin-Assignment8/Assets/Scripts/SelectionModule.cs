using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionModule : MonoBehaviour
{
    /*
        SelectionModule represents the backend of the smaller console in the main room, where the user can toggle which track they want to edit.
    */

    [SerializeField] GameObject[] buttons; // array of buttons on the console
    [SerializeField] GameObject[] consoles; // array of larger DAW consoles existing in the scene (only one will be visible at any given time, but six are necessary for the six tracks in the experience)

    // Start is called before the first frame update
    void Start() // By default, Track I will be the immediately editable track
    {
        buttons[0].GetComponent<ButtonModule>().isPressed = true;
    }

    public void ChangeButton(GameObject button) // this method is called whenever a button with "selectionConsole" true in ButtonModule becomes the "pressedObject" in InteractionModule
    {
        for (int i = 0; i < buttons.Length; i++) // iterate through the array of buttons
        {
            if (buttons[i] == button) // if the button passed to the method matches the current element in the array, set the element in the array as active (this causes its DAW console to appear)
                consoles[i].SetActive(true);

            else // if the button passed to the method differs from the current element in the array, ensure that the current element is inactive
            {
                buttons[i].GetComponent<ButtonModule>().isPressed = false;
                consoles[i].SetActive(false);
            }
        }
    }
}