using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineDriveTrain : MonoBehaviour
{
    // Drivetrain game objects
    private SubmarineControlConsole controlConsole;
    private SubmarinePowerTrain powerTrain;

    // Start is called before the first frame update
    void Start()
    {
        controlConsole = GetComponentInChildren<SubmarineControlConsole> ();
        if (controlConsole == null)
        {
            Debug.Log("Submarine missing its controlConsole. Destroying this drive train");
            Destroy(this);
            return;
        }

        powerTrain = GetComponentInChildren<SubmarinePowerTrain>();
        if (powerTrain == null)
        {
            Debug.Log("Submarine missing its powerTrain. Destroying this drive train");
            Destroy(this);
            return;
        }

        // Imagine wiring a throttle to an engine fuel-injector (speed) and some servos (direction)
        powerTrain.connectControls(controlConsole.throttleL, controlConsole.throttleR);
    }

    public Vector3 getForceLinear()
    {
        return powerTrain.getForceLinear();
    }
    public Vector3 getTorque()
    {
        return powerTrain.getTorque();
    }

}
