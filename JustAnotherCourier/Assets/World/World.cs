using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class World : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("velo: " + GetComponent<Rigidbody>().velocity.ToString());

        //GetComponent<Rigidbody>().velocity = Vector3.zero;
        //GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
