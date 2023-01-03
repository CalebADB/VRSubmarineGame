using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SubmarineControlConsole : MonoBehaviour
{
    // Throttles
    public SubmarineThrottle throttleL;
    public SubmarineThrottle throttleR;

    // Oz (simulated player interaction)
    [SerializeField]
    public bool shouldOz = true;
    [SerializeField]
    public bool shouldOzFwd = false;
    [SerializeField]
    public bool shouldOzBwd = false;
    [SerializeField]
    public bool shouldOzUp = false;
    [SerializeField]
    public bool shouldOzDown = false;
    [SerializeField]
    public bool shouldOzLeft = false;
    [SerializeField]
    public bool shouldOzRight = false;

    void Start()
    {
        SubmarineThrottle[] throttles = GetComponentsInChildren<SubmarineThrottle>(); ;
        foreach (SubmarineThrottle throttle in throttles)
        {
            if (throttle.name == "ThrottleL") throttleL = throttle;
            if (throttle.name == "ThrottleR") throttleR = throttle;
        }
    }

    void Update()
    {
        manageOzEffect();
    }

    // Testing function a la "wizard of Oz." Focussing on linear movement
    void manageOzEffect()
    {
        // Turn off all oz effects for all controls in the control console if indicated
        if (!shouldOz)
        {
            throttleL.shouldApplyOzForceToHandleP10_000_000 = false;
            throttleL.shouldApplyOzForceToHandleN10_000_000 = false;
            throttleL.shouldApplyOzForceToHandle000_P10_000 = false;
            throttleL.shouldApplyOzForceToHandle000_N10_000 = false;
            throttleL.shouldApplyOzForceToHandle000_000_N10 = false;
            throttleL.shouldApplyOzForceToHandle000_000_P10 = false;
            
            throttleR.shouldApplyOzForceToHandleP10_000_000 = false;
            throttleR.shouldApplyOzForceToHandleN10_000_000 = false;
            throttleR.shouldApplyOzForceToHandle000_P10_000 = false;
            throttleR.shouldApplyOzForceToHandle000_N10_000 = false;
            throttleR.shouldApplyOzForceToHandle000_000_N10 = false;
            throttleR.shouldApplyOzForceToHandle000_000_P10 = false;
            return;
        }

        // Set pairs of effects, resulting in linear forces being applied on submarine
        if (shouldOzLeft)
        {
            throttleL.shouldApplyOzForceToHandleP10_000_000 = true;
            throttleR.shouldApplyOzForceToHandleP10_000_000 = true;
        }
        else
        {
            throttleL.shouldApplyOzForceToHandleP10_000_000 = false;
            throttleR.shouldApplyOzForceToHandleP10_000_000 = false;
        }
        if (shouldOzRight)
        {
            throttleL.shouldApplyOzForceToHandleN10_000_000 = true;
            throttleR.shouldApplyOzForceToHandleN10_000_000 = true;
        }
        else
        {
            throttleL.shouldApplyOzForceToHandleN10_000_000 = false;
            throttleR.shouldApplyOzForceToHandleN10_000_000 = false;
        }
        if (shouldOzUp)
        {
            throttleL.shouldApplyOzForceToHandle000_P10_000 = true;
            throttleR.shouldApplyOzForceToHandle000_P10_000 = true;
        }
        else
        {
            throttleL.shouldApplyOzForceToHandle000_P10_000 = false;
            throttleR.shouldApplyOzForceToHandle000_P10_000 = false;
        }
        if (shouldOzDown)
        {
            throttleL.shouldApplyOzForceToHandle000_N10_000 = true;
            throttleR.shouldApplyOzForceToHandle000_N10_000 = true;
        }
        else
        {
            throttleL.shouldApplyOzForceToHandle000_N10_000 = false;
            throttleR.shouldApplyOzForceToHandle000_N10_000 = false;
        }
        if (shouldOzFwd)
        {
            throttleL.shouldApplyOzForceToHandle000_000_N10 = true;
            throttleR.shouldApplyOzForceToHandle000_000_N10 = true;
        }
        else
        {
            throttleL.shouldApplyOzForceToHandle000_000_N10 = false;
            throttleR.shouldApplyOzForceToHandle000_000_N10 = false;
        }
        if (shouldOzBwd)
        {
            throttleL.shouldApplyOzForceToHandle000_000_P10 = true;
            throttleR.shouldApplyOzForceToHandle000_000_P10 = true;
        }
        else
        {
            throttleL.shouldApplyOzForceToHandle000_000_P10 = false;
            throttleR.shouldApplyOzForceToHandle000_000_P10 = false;
        }
    }
}
