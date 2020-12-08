using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionModule : MonoBehaviour
{
    private bool grabbed, pressed;
    public string hand;
    public GameObject grabbedObject, pressedObject, console;
    public Material grabHighlight, pressHighlight;
    public List<GameObject> currentCollisions;

    // Start is called before the first frame update
    void Start()
    {
        currentCollisions = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((!OVRInput.Get(OVRInput.RawButton.LHandTrigger) && hand == "Left") || (!OVRInput.Get(OVRInput.RawButton.RHandTrigger) && hand == "Right")) // if the user lets go of (or does not press) the Hand Trigger button, set the currently grabbed object to null
        {
            grabbed = false;
            grabbedObject = null;
        }

        if (grabbed && grabbedObject != null) // if the user is currently holding an object (either a slider or a door handle)
        {
            if (grabbedObject.GetComponent<SliderModule>() != null) // if the user is holding a slider, then clamp its position based on the x axis limits serialized in the slider's personal module
            {
                grabbedObject.transform.position = new Vector3(Mathf.Clamp(transform.position.x, grabbedObject.GetComponent<SliderModule>().GetMinX(), grabbedObject.GetComponent<SliderModule>().GetMaxX()), grabbedObject.transform.position.y, grabbedObject.transform.position.z);
            }

            else if (grabbedObject.tag == "Handle") // otherwise, if the user is holding an invisible "GrabbableHandle" object, simply assign the position and rotation of the user's hand to the handle
            {
                grabbedObject.transform.position = gameObject.transform.position;
                grabbedObject.transform.rotation = gameObject.transform.rotation;
            }
        }

        if (pressed && pressedObject != null) // if the user is currently pressing a button (buttons cannot be grabbed, but their states can be changed)
        {
            if (pressedObject.GetComponent<ButtonModule>() != null) // ensure that the button has a ButtonModule script attached (this needs to be done manually in Unity's Inspector window)
            {
                pressedObject.GetComponent<ButtonModule>().isPressed = !pressedObject.GetComponent<ButtonModule>().isPressed; // flip the value of the "isPressed" Boolean attribute in the button's ButtonModule

                if (pressedObject.GetComponent<ButtonModule>().selectionConsole) // check if this particular button is on the track selection console
                    console.GetComponent<SelectionModule>().ChangeButton(pressedObject); // if so, we need to change which of the six DAW consoles is currently active in SelectionModule
            }

            pressed = false; // immediately reset / nullify the pressed values, since we only care about the FIRST frame that the Index Trigger is pressed
            pressedObject = null;
        }

        if (pressedObject == null) // if the user has not pressed a button with the particular controller
        {
            if (!pressed && ((OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && hand == "Left") || (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && hand == "Right")))
            { // if a Button is not currently being interacted with on the FIRST frame that the Index Trigger is pressed (this is why we use GetDown instead of Get)
                foreach (GameObject obj in currentCollisions)
                {
                    if (obj.layer == 10) // iterate through the list of objects with which the player's hand is currently in contact, until we find one with the "pressable" layer
                    {
                        obj.GetComponent<Renderer>().material = obj.GetComponent<InitialMaterial>().GetInitialMaterial(); // if we find such an object, reset its material
                        pressed = true; // indicate to the module that an object is currently pressed
                        pressedObject = obj;
                        break; // exit the loop
                    }
                }
            }
        }

        if (grabbedObject == null) // if the user has not grabbed an object with the particular controller
        {
            if (!grabbed && ((OVRInput.Get(OVRInput.RawButton.LHandTrigger) && hand == "Left") || (OVRInput.Get(OVRInput.RawButton.RHandTrigger) && hand == "Right")))
            { // if an object is not currently being held on ANY frame that the Hand Trigger is pressed (this is why we use Get)
                foreach (GameObject obj in currentCollisions)
                {
                    if (obj.layer == 9) // iterate through the list of objects with which the player's hand is currently in contact, until we find one with the "grabbable" tag
                    {
                        if (obj.tag == "Handle") // if we find such an object, and this object is a handle, then reset the material of its visible parent
                            obj.transform.parent.gameObject.GetComponent<Renderer>().material = obj.GetComponent<InitialMaterial>().GetInitialMaterial();

                        else obj.GetComponent<Renderer>().material = obj.GetComponent<InitialMaterial>().GetInitialMaterial(); // otherwise, reset the material of the object itself
                        grabbed = true; // indicate to the module that an object has been grabbed / picked up
                        grabbedObject = obj;
                        break; // exit the loop
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider col) // This method is executed whenever the Trigger collider on the player's hand touches the collider on another object
    {
        if (col.gameObject.layer == 9) // grabbable layer
        {
            if (col.gameObject != this.grabbedObject) // we don't want to highlight an object that the user is already holding
            {
                if (col.gameObject.tag == "Handle") // if the user is hovering over a door handle, we need to highlight its parent (the actual grab-box is an invisible child of the visible handle)
                    col.gameObject.transform.parent.gameObject.GetComponent<Renderer>().material = grabHighlight;

                else col.gameObject.GetComponent<Renderer>().material = grabHighlight; // otherwise, highlight the object itself
            }

            currentCollisions.Add(col.gameObject); // add the object to a list of "current collisions," which will be iterated through when the grab button is pressed
        }

        else if (col.gameObject.layer == 10) // pressable layer
        {
            if (col.gameObject != this.pressedObject) // as long as the object is not currently pressed, highlight it
                col.gameObject.GetComponent<Renderer>().material = pressHighlight;

            currentCollisions.Add(col.gameObject); // add the object to the same list described in line 106
        }
    }

    void OnTriggerExit(Collider col) // This method is executed on the exact frame that the hand Trigger collider breaks contact with another object
    {
        if (col.gameObject.layer == 9 || col.gameObject.layer == 10) // if the object in question is grabbable or pressable
        {
            if (col.gameObject.tag == "Handle") // if the object is a door handle, then reset the material of its visible parent
                col.gameObject.transform.parent.gameObject.GetComponent<Renderer>().material = col.gameObject.GetComponent<InitialMaterial>().GetInitialMaterial();

            else col.gameObject.GetComponent<Renderer>().material = col.gameObject.GetComponent<InitialMaterial>().GetInitialMaterial(); // otherwise, reset the material of the object itself

            currentCollisions.Remove(col.gameObject); // remove the object from the "current collisions" list to prevent it from being grabbed or pressed
        }
    }
}
