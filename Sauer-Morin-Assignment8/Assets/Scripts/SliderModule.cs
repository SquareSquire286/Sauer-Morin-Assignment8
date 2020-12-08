using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    SliderModule essentially only exists to explicitly record the minimal and maximal positions of each slider in world space. This is necessary in order
    for ChannelModule to perform calculations based on the slider's distance from the maximal position (relative to the minimal position). These calculations
    are then converted to the absolute values that the user sees.
*/
public class SliderModule : MonoBehaviour
{
    [SerializeField] float minX; // The minimum position along the X axis that the slider can occupy
    [SerializeField] float maxX; // The maximum position along the X axis that the slider can occupy

    public float GetMinX() // simple getter for the minX attribute
    {
        return minX;
    }

    public float GetMaxX() // simple getter for the maxX attribute
    {
        return maxX;
    }

    public float GetRange() // getter for the difference between maxX and minX, allowing the backend to perform percentage calculations in ChannelModule
    {
        return (maxX - minX);
    }
}