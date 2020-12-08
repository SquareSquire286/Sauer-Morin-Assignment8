using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPhysics : MonoBehaviour
{
    /*
        FollowPhysics is an essential component in enabling the user to open doors. It causes the visible handle (which, unbeknownst to the user, is non-interactable) to
        follow the position of an invisible grabbable box identical to the handle in size, while also conforming to the restrictions of the door's Hinge Joint.
    */
    public Transform target; // the Transform of the object that the visible handle must follow (in this case, it is the invisible "GrabbableHandle")
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // ensure that the handle's Rigidbody is assigned to the rb attribute
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(target.transform.position); // update the position of the handle's Rigidbody to match that of the target attribute
    }
}
