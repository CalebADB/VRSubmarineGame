using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Submarine : MonoBehaviour
{
    // World
    private World world;

    // Submarine game objects
    private SubmarineFrame frame;
    private SubmarineDrivetrain drivetrain;

    // physics
    float gravitationalCoefficient = 1000.0f;
    [SerializeField]
    Vector3 centerOfGravity = Vector3.down;
    float momentOfInertia = 0.01f;

    [SerializeField]
    Vector3 velocityLinear = Vector3.zero;
    [SerializeField]
    Vector3 velocityAngular = Vector3.zero;
    [SerializeField, Range(0.1f, 500)]
    float speedTerminalLinear = 1;
    [SerializeField, Range(0.1f, 500)]
    float speedAngularMax = 1;

    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.FindGameObjectWithTag("World").GetComponent<World>();
        if (world == null)
        {
            Debug.Log("Submarine missing its world. Destroying submarine");
            Destroy(this);
            return;
        }

        frame = GetComponentInChildren<SubmarineFrame>();
        if (frame == null)
        {
            Debug.Log("Submarine missing its frame. Destroying submarine");
            Destroy(this);
            return;
        }

        drivetrain = GetComponentInChildren<SubmarineDrivetrain>();
        if (drivetrain == null)
        {
            Debug.Log("Submarine missing its drivetrain. Destroying submarine");
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Dont manage player frame of reference here. Create physics manager, and use that behaviour attach player to their frame.
        
        // Calculate effects on submarine
        calculateGravitationalForce();

        // Calculate displacement
        calculateVelocityLinear();
        calculateVelocityAngular();
        displace();
    }

    public Vector3 getVelocityLinear()
    {
        return velocityLinear;
    }
    public Vector3 getVelocityAngular()
    {
        return velocityAngular;
    }

    private void calculateGravitationalForce()
    {
        // Find angle between world gravitational pull and ship ballast
        Vector3 centerOfGravityRelativeToWorld = Quaternion.Inverse(world.transform.rotation) * centerOfGravity;
        Vector3 gravitationalTorque = Vector3.Cross(centerOfGravity, centerOfGravityRelativeToWorld);
        velocityAngular += gravitationalTorque * gravitationalCoefficient * Time.deltaTime;
    }

    private void calculateVelocityLinear()
    {
        // Apply force from submarine components
        velocityLinear += drivetrain.getForceLinear() * Time.deltaTime;
        velocityLinear += frame.getForceLinear() * Time.deltaTime;

        // Bind speed to max value in editor
        float speedLinear = velocityLinear.magnitude;
        if (speedLinear > speedTerminalLinear) velocityLinear *= (speedTerminalLinear / speedLinear);
    }
    private void calculateVelocityAngular()
    {
        // Apply force from submarine components
        velocityAngular += (drivetrain.getTorque() / momentOfInertia) * Time.deltaTime;
        velocityAngular += (frame.getTorque() / momentOfInertia) * Time.deltaTime;

        // Bind speed to max value in editor
        float speedAngular = velocityAngular.magnitude;
        if (speedAngular > speedAngularMax) velocityAngular *= (speedAngularMax / speedAngular);
    }
    private void displace()
    {
        // Displace position
        Vector3 displacemenLinear = velocityLinear * Time.deltaTime;
        world.transform.position += -displacemenLinear;

        // Displace rotation
        Vector3 displacementAngular = velocityAngular * Time.deltaTime;
        world.transform.RotateAround(Vector3.zero, Vector3.right, displacementAngular.x);
        world.transform.RotateAround(Vector3.zero, Vector3.up, displacementAngular.y);
        world.transform.RotateAround(Vector3.zero, Vector3.forward, displacementAngular.z);
    }
}
