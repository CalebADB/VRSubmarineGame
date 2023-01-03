using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarinePowerTrain : MonoBehaviour
{
    private SubmarineEngine engineL;
    private SubmarineEngine engineR;

    private Vector3 forceLinear = Vector3.zero;
    private Vector3 torque = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        SubmarineEngine[] engines = GetComponentsInChildren<SubmarineEngine>(); ;
        foreach (SubmarineEngine engine in engines)
        {
            if (engine.name == "EngineL") engineL = engine;
            if (engine.name == "EngineR") engineR = engine;
        }
    }

    void Update()
    {
        // Calculate effects on powerTrain
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

    public void connectControls(SubmarineThrottle throttleL, SubmarineThrottle throttleR)
    {
        throttleL.connectEngine(engineL);
        throttleR.connectEngine(engineR);
    }

    private void calculateForce()
    {
        torque = Vector3.zero;
        forceLinear = Vector3.zero;

        addEngineForce();
    }

    private void addEngineForce()
    {
        // Get all values from engines
        Vector3 engineLTorque = engineL.getTorque();
        Vector3 engineRTorque = engineR.getTorque();
        Vector3 engineTorque = engineLTorque + engineRTorque;
        float totalTorqueMag = (engineLTorque.magnitude + engineRTorque.magnitude);
        // absorbedTorque transfers it's energy into a linear thrust because of the values of the torques which are balancing
        float absorbedTorqueMag = totalTorqueMag - engineTorque.magnitude;
        
        // Force that will move the object in a direction
        forceLinear += engineL.getForceLinear();
        forceLinear += engineR.getForceLinear();
        forceLinear += (engineL.getTangentialForce() + engineR.getTangentialForce()).normalized * absorbedTorqueMag;

        // Force that will move the object in a rotation
        if (Mathf.Abs(totalTorqueMag) > 0.001f) torque += engineTorque * ((totalTorqueMag - absorbedTorqueMag) / totalTorqueMag);
    }
}
