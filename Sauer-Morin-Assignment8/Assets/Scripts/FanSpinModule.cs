using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSpinModule : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 359 * Time.deltaTime); // apply a constant rotation to the fan
    }
}
