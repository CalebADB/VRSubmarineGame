using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : MonoBehaviour
{
    private SubmarineFrame frame;
    private SubmarineDriveTrain driveTrain;

    // Start is called before the first frame update
    void Start()
    {
        frame = GetComponentInChildren<SubmarineFrame>();
        if (frame == null)
        {
            Debug.Log("Submarine missing its frame. Destroying submarine");
            Destroy(this);
            return;
        }
        
        driveTrain = GetComponentInChildren<SubmarineDriveTrain>();
        if (driveTrain == null)
        {
            Debug.Log("Submarine missing its driveTrain. Destroying submarine");
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
