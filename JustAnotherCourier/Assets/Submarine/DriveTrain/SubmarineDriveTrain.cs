using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineDrivetrain : MonoBehaviour
{
    // Drivetrain game objects
    private SubmarineControlConsole controlConsole;
    private SubmarinePowertrain powertrain;

    // Start is called before the first frame update
    void Awake()
    {
        controlConsole = GetComponentInChildren<SubmarineControlConsole>();
        if (controlConsole == null)
        {
            Debug.Log("Drivetrain missing its controlConsole");
            return;
        }

        powertrain = GetComponentInChildren<SubmarinePowertrain>();
        if (powertrain == null)
        {
            Debug.Log("Drivetrain missing its powertrain");
            return;
        }
    }
    void Start()
    {         
        // Imagine wiring a throttle to an engine fuel-injector (speed) and some servos (direction)
        powertrain.connectControls(controlConsole.throttleL, controlConsole.throttleR);
    }


    public Vector3 getForceLinear()
    {
        return powertrain.getForceLinear();
    }
    public Vector3 getTorque()
    {
        return powertrain.getTorque();
    }

}
