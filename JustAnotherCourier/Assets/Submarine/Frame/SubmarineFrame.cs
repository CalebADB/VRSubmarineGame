using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SubmarineFrame : MonoBehaviour
{
    // Parent submarine
    private Submarine parentSubmarine;

    // Physics
    private float dragLinearCoefficient = 0.6f;
    private float dragAngularCoefficient = 0.15f;

    private Vector3 forceLinear;
    private Vector3 torque;

    // Start is called before the first frame update
    void Awake()
    {
        parentSubmarine = GameObject.FindGameObjectWithTag("Submarine").GetComponent<Submarine>();
        if (parentSubmarine == null)
        {
            Debug.Log("Frame missing its submarine");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate effects on submarineFrame
        calculateForce();
    }

    public Vector3 getForceLinear()
    {
        return forceLinear;
    }
    public Vector3 getTorque()
    {
        return torque;
    }

    private void calculateForce()
    {
        forceLinear = Vector3.zero;
        torque = Vector3.zero;

        addDragForce();
    }
    private void addDragForce()
    {
        Vector3 velocityLinear = parentSubmarine.getVelocityLinear();
        Vector3 velocityAngular = parentSubmarine.getVelocityAngular();

        // drag equation 
        forceLinear += -velocityLinear.normalized * velocityLinear.sqrMagnitude * dragLinearCoefficient;

        // pseudo drag equation (taking velocityAngular.magnitude instead of velocityAngular.sqrMagnitudebecause of stability issues with physics and framerate) 
        torque += -velocityAngular.normalized * velocityAngular.magnitude * dragAngularCoefficient;
    }
}
