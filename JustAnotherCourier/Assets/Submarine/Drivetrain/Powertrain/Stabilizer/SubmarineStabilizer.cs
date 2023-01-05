using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SubmarineStabilizer : MonoBehaviour
{
    // Parent submarine
    private Submarine parentSubmarine;

    // Controls
    private Dictionary<string, bool> mapFromControlToIsBeingInteracted = new Dictionary<string, bool>();

    // Physics
    private float stablizeThrustFactor = 300.0f;
    // the max angular speed for the stabilzer to be turned on
    private float speedAngularMax = 0.9f;

    // Start is called before the first frame update
    void Awake()
    {
        parentSubmarine = GameObject.FindGameObjectWithTag("Submarine").GetComponent<Submarine>();
        if (parentSubmarine == null)
        {
            Debug.Log("Stabilizer missing its parentSubmarine");
            return;
        }

        mapFromControlToIsBeingInteracted.Add("ThrottleL", false);
        mapFromControlToIsBeingInteracted.Add("ThrottleR", false);
    }

    public void control(string name, bool isBeingInteracted)
    {
        if(mapFromControlToIsBeingInteracted.ContainsKey(name)) mapFromControlToIsBeingInteracted[name] = isBeingInteracted;
        else Debug.LogError("Control: " + name + " is not in the map: mapFromControlToIsBeingInteracted");
    }
    public Vector3 getTorque()
    {
        // Check submarine angular speed is low enough to engage stabilizer
        if (parentSubmarine.getVelocityAngular().magnitude > speedAngularMax) return Vector3.zero;

        // Check all submarine controls are not being to engage stabilizer
        foreach (bool controlIsBeingInteracted in mapFromControlToIsBeingInteracted.Values) if (controlIsBeingInteracted) return Vector3.zero;

        // Simulate gravitation force with artificial factor: stablizeThrustFactor
        Vector3 centerOfGravityRelativeToWorld = parentSubmarine.world.transform.rotation * parentSubmarine.centerOfGravity;

        return -Vector3.Cross(parentSubmarine.centerOfGravity, centerOfGravityRelativeToWorld) * stablizeThrustFactor;
    }
}
