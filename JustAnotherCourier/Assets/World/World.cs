using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Rigidbody))]
public class World : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("velo: " + rb.velocity.ToString() + ", aVelo: " + rb.angularVelocity.ToString() + ", dCol: " + rb.detectCollisions.ToString());// + " : "rigidbody.velocity.ToString())
        // Dont manage player frame of reference here. Create physics manager, and attach player to their frame.

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //rigidbody.angularVelocity = Vector3.zero;
    }


}
