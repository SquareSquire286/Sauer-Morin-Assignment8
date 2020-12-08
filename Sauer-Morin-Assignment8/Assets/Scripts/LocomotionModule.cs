using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    LocomotionModule controls the user's ability to artificially move around the virtual environment using the left Oculus Touch controller's thumbstick.
*/

public class LocomotionModule : MonoBehaviour
{
    [SerializeField] GameObject camera;
    [SerializeField] float speed;
    private bool enabled;

    // Start is called before the first frame update
    void Start()
    {
        if (camera == null) // If the camera property has not been set, prevent the entire module from functioning; otherwise, allow the module to function
            enabled = false;

        else enabled = true;

        if (speed == null) // if the speed property has not been set, assign it an arbitrary value of 2 
            speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled) // the locomotion module will only function if the camera property has been set (otherwise, there is nothing to move!)
        {
            var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick); // read input from the left thumbstick (returns a two-variable vector with values between -1 and 1)
            float fixedY = camera.GetComponent<Rigidbody>().position.y; // lock the camera's position along the Y axis to preserve the "height" of the virtual avatar

            camera.GetComponent<Rigidbody>().position += (transform.right * joystickAxis.x + transform.forward * joystickAxis.y) * Time.deltaTime * speed; // apply changes to the user's position along the X and Z axes
            camera.GetComponent<Rigidbody>().position = new Vector3(camera.GetComponent<Rigidbody>().position.x, fixedY, camera.GetComponent<Rigidbody>().position.z); // formally update the user's position while locking the Y axis

            if (joystickAxis.x == 0 && joystickAxis.y == 0) // if the user is not moving the thumbstick, prevent any artificial locomotion from occurring
                camera.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}
