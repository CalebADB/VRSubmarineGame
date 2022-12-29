using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SubmarineControlConsole : MonoBehaviour
{
    SubmarineThrottle throttleL;
    SubmarineThrottle throttleR;

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
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if ((gameObject.name == "ThrottleL") && (gameObject.GetComponent<SubmarineThrottle>() != null)) throttleL = gameObject.GetComponent<SubmarineThrottle>();
            if ((gameObject.name == "ThrottleR") && (gameObject.GetComponent<SubmarineThrottle>() != null)) throttleR = gameObject.GetComponent<SubmarineThrottle>();
        }

    }

    void Update()
    {
        manageOzEffect();
    }

    void manageOzEffect()
    {
        if (!shouldOz)
        {
            throttleL.shouldApplyOzForceToHandle000_000_N10 = false;
            throttleR.shouldApplyOzForceToHandle000_000_N10 = false;
            throttleL.shouldApplyOzForceToHandle000_000_P10 = false;
            throttleR.shouldApplyOzForceToHandle000_000_P10 = false;
            throttleL.shouldApplyOzForceToHandle000_P10_000 = false;
            throttleR.shouldApplyOzForceToHandle000_P10_000 = false;
            throttleL.shouldApplyOzForceToHandle000_N10_000 = false;
            throttleR.shouldApplyOzForceToHandle000_N10_000 = false;
            throttleL.shouldApplyOzForceToHandleP10_000_000 = false;
            throttleR.shouldApplyOzForceToHandleP10_000_000 = false;
            throttleL.shouldApplyOzForceToHandleN10_000_000 = false;
            throttleR.shouldApplyOzForceToHandleN10_000_000 = false;
            return;
        }

        if (shouldOzFwd)
        {
            throttleL.shouldApplyOzForceToHandle000_000_N10 = true;
            throttleR.shouldApplyOzForceToHandle000_000_N10 = true;
        }
        if (shouldOzBwd)
        {
            throttleL.shouldApplyOzForceToHandle000_000_P10 = true;
            throttleR.shouldApplyOzForceToHandle000_000_P10 = true;
        }
        if (shouldOzUp)
        {
            throttleL.shouldApplyOzForceToHandle000_P10_000 = true;
            throttleR.shouldApplyOzForceToHandle000_P10_000 = true;
        }
        if (shouldOzDown)
        {
            throttleL.shouldApplyOzForceToHandle000_N10_000 = true;
            throttleR.shouldApplyOzForceToHandle000_N10_000 = true;
        }
        if (shouldOzLeft)
        {
            throttleL.shouldApplyOzForceToHandleP10_000_000 = true;
            throttleR.shouldApplyOzForceToHandleP10_000_000 = true;
        }
        if (shouldOzRight)
        {
            throttleL.shouldApplyOzForceToHandleN10_000_000 = true;
            throttleR.shouldApplyOzForceToHandleN10_000_000 = true;
        }

    }
}
