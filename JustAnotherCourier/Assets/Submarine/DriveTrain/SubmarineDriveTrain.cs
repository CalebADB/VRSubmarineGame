using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineDriveTrain : MonoBehaviour
{
    private SubmarineControlConsole controlConsole;
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
