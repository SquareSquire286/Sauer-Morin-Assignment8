using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorModule : MonoBehaviour
{
    bool isGrabbed;
    public GameObject parentDoor, parentHandle, leftHand, rightHand, centerEyeAnchor;

    // Start is called before the first frame update
    void Start()
    {
        isGrabbed = false; // By default, no doors are being held by the user
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject == leftHand.GetComponent<InteractionModule>().grabbedObject || gameObject == rightHand.GetComponent<InteractionModule>().grabbedObject)
            isGrabbed = true; // isGrabbed will be true if InteractionModule recognizes the invisible GrabbableHandle as either hand's currently grabbed object

        else isGrabbed = false;

        if (!isGrabbed) // if the handle is not currently grabbed, prevent the physical door from moving along any of the six axes, and enable all colliders to prevent the player from passing through the door
        {
            transform.position = parentHandle.transform.position;
            transform.rotation = parentHandle.transform.rotation;
            parentDoor.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            parentDoor.GetComponent<Collider>().isTrigger = false;
            parentHandle.GetComponent<Collider>().isTrigger = false;
            centerEyeAnchor.GetComponent<Collider>().isTrigger = false;
        }

        else // if the handle is currently grabbed, disable the previous Rigidbody constraints, and make colliders on all involved objects event-triggered rather than physics-based (prevents the Rigidbody on the camera from clashing with the Rigidbody on the door and preventing the player from proceeding)
        {
            parentDoor.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            parentDoor.GetComponent<Collider>().isTrigger = true;
            parentHandle.GetComponent<Collider>().isTrigger = true;
            centerEyeAnchor.GetComponent<Collider>().isTrigger = true;
        }
    }
}
