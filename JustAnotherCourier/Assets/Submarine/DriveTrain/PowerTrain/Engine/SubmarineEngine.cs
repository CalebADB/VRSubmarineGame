using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineEngine : MonoBehaviour
{
    // Engine body
    Transform bodyTransform;

    // Physics
    Vector3 thrustDirection = Vector3.zero;
    float thrustMagnitude = 0;

    [SerializeField]
    private float thrustMagnitudeMax = 300.0f;
    [SerializeField]
    public Vector3 relativeLocation = Vector3.zero; //Relative to moment of inertia

    // Start is called before the first frame update
    void Awake()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms) if (t.gameObject.name == "Body") bodyTransform = t;
        if (bodyTransform.name != "Body")
        {
            Debug.Log("Engine missing its body");
            return;
        }
    }

    public void control(Vector3 controlDirection, float thrustRatio)
    {
        // Set direction (for physics calculations)
        thrustDirection = controlDirection;

        // Set thrust (for physics calculations)
        if (thrustRatio > 1.0f) thrustMagnitude = thrustMagnitudeMax;
        else thrustMagnitude = thrustRatio * thrustMagnitudeMax;

        // Rotate engine body to match throttle directin
        if (thrustRatio > 0.001f) bodyTransform.rotation = Quaternion.LookRotation(-controlDirection, Vector3.up);
    }

    public Vector3 getTotalForce()
    {
        return thrustDirection * thrustMagnitude;
    }
    // Returns the force tangent to the vector from the center of the ship to the location of the engine
    public Vector3 getTangentialForce()
    {
        return Vector3.ProjectOnPlane(getTotalForce(), relativeLocation);
    }
    public Vector3 getForceLinear()
    {
        return getTotalForce() - getTangentialForce();
    }
    public Vector3 getTorque()
    {
        return Vector3.Cross(getTangentialForce(), relativeLocation);
    }
}
