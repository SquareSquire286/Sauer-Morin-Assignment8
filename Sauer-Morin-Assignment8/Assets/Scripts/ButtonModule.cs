using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ButtonModule handles all native functionalities of buttons. There is only one such functionality; a button's discrete position will be altered each time it is pressed.
*/
public class ButtonModule : MonoBehaviour
{
    public bool selectionConsole; // this is necessary for SelectionModule, as not every button appears on the track selection console. Others simply control the output of an audio effect.
    [SerializeField] Vector3 offPosition;
    [SerializeField] Vector3 onPosition;
    public bool isPressed;
    public bool isEcho; // the Echo button must turn off if the track is muted, as the echoed audio will still play based on the echo delay

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed) // isPressed will become true in InteractionModule if the button is pressed
        {
            if (!isEcho || (isEcho && !GameObject.Find("Mute Button").GetComponent<ButtonModule>().isPressed))
                transform.position = onPosition;

            else if (isEcho && GameObject.Find("Mute Button").GetComponent<ButtonModule>().isPressed)
                isPressed = false;
        }

        else transform.position = offPosition;
    }
}