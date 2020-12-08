using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    The initialMaterial property is used to reset the material of a grabbable or pressable object when the user's hand ceases hovering over it.
*/

public class InitialMaterial : MonoBehaviour
{
    [SerializeField] Material initialMaterial;

    public Material GetInitialMaterial() // A standard getter for the initial material of the object (used extensively in InteractionModule)
    {
        return initialMaterial;
    }
}