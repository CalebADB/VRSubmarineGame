using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SubmarineThrottle : MonoBehaviour
{
    // Hand
    [SerializeField]
    private SteamVR_Input_Sources handType;

    // Handle visual
    private Transform handleVisualTransform;

    // Handle interactable
    private SubmarineThrottleHandleInteractable handleInteractable;
    private Transform handleInteractableTransform;

    // Engine
    private SubmarineEngine engine;

    // Physics
    private Vector3 handleVelocity = Vector3.zero;

    private Vector3 handleDisplacement ;
    private float handleDistance;

    private Vector3 handleOriginPosition = Vector3.zero;
    private Quaternion handleOriginRotation = Quaternion.identity;

    [SerializeField, Range(0, 1)]
    private float handleRadiusMax;
    [SerializeField, Range(0, 1)]
    private float handleTensionRadiusMin;
    [SerializeField, Range(0, 1)]
    private float handleTensionFactor;
    [SerializeField, Range(0, 0.1f)]
    private float handleDisplacementTolerance = 0.001f;
    [SerializeField, Range(0, 500)]
    private float handleReturnForceCoefficient = 100.0f;

    // Oz (simulated player interaction)
    [SerializeField]
    public bool shouldApplyOzForceToHandle = true;
    [SerializeField, Range(0, 10)]
    public float ozForceFactor = 1.0f;
    [SerializeField]
    public bool shouldApplyOzForceToHandleP10_000_000 = false;
    Vector3 ozForceToHandleP10_000_000 = new Vector3(10.0f, 0.0f, 0.0f);
    [SerializeField]
    public bool shouldApplyOzForceToHandleN10_000_000 = false;
    Vector3 ozForceToHandleN10_000_000 = new Vector3(-10.0f, 0.0f, 0.0f);
    [SerializeField]
    public bool shouldApplyOzForceToHandle000_P10_000 = false;
    Vector3 ozForceToHandle000_P10_000 = new Vector3(0.0f, 10.0f, 0.0f);
    [SerializeField]
    public bool shouldApplyOzForceToHandle000_N10_000 = false;
    Vector3 ozForceToHandle000_N10_000 = new Vector3(0.0f, -10.0f, 0.0f);
    [SerializeField]
    public bool shouldApplyOzForceToHandle000_000_P10 = false;
    Vector3 ozForceToHandle000_000_P10 = new Vector3(0.0f, 0.0f, 10.0f);
    [SerializeField]
    public bool shouldApplyOzForceToHandle000_000_N10 = false;
    Vector3 ozForceToHandle000_000_N10 = new Vector3(0.0f, 0.0f, -10.0f);


    // Start is called before the first frame update
    void Start()
    {
        handleOriginPosition = transform.position;
        
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        foreach (Transform childTransform in childTransforms)
        {
            if (childTransform.gameObject.name == "HandleVisual") handleVisualTransform = childTransform;
            if (childTransform.gameObject.name == "HandleInteractable") handleInteractableTransform = childTransform;
        }

        Debug.Assert((handleVisualTransform != null), "handleVisualTransform is null");
        Debug.Assert((handleInteractableTransform != null), "handleInteractableTransform is null");

        handleInteractable = handleInteractableTransform.GetComponent<SubmarineThrottleHandleInteractable>();
    }

    void Update()
    {
        // Calculate where handle visual and hancle interactable are
        CalculateHandleDisplacement();
        
        // Determine what forces should effect throttle based on player input
        if (handleInteractable.isHandAttached)
        {
            handleVisualTransform.position = handleOriginPosition + handleDisplacement;
        }
        else
        {
            ApplyHandleReturnForce();
            ApplyHandleOzForce();

            handleVisualTransform.position += handleVelocity * Time.deltaTime;
            handleInteractableTransform.position = handleVisualTransform.position;
        }

        engine.control(handleDisplacement.normalized, handleDistance / handleRadiusMax);
    }

    private void FixedUpdate()
    {
        // Determine what forces should effect throttle based on player input
        if (handleInteractable.isHandAttached)
        {
            ApplyHandleVibration();
        }
    }
    
    public void connectEngine(SubmarineEngine inEngine)
    {
        engine = inEngine;
    }

    private void CalculateHandleDisplacement()
    {
        // Determine if handle visual is directly connected to the handle interactable (based on handle "tension")
        Vector3 handleInteractableDisplacement = handleInteractableTransform.position - handleOriginPosition;
        float handleInteractableDistance = handleInteractableDisplacement.magnitude;
        if (handleInteractableDistance > handleTensionRadiusMin)
        {
            // Determine handle visual position based on handle interactable position (based on handle "tension")
            Vector3 handleInteractableDirection = handleInteractableDisplacement.normalized;
            handleDisplacement = (handleInteractableDirection * handleTensionRadiusMin) + (handleInteractableDirection * (handleInteractableDistance - handleTensionRadiusMin) * handleTensionFactor);
            handleDistance = handleDisplacement.magnitude;

            // Bind handle visual to fixed length
            if (handleDistance > handleRadiusMax) handleDisplacement = handleDisplacement.normalized * handleRadiusMax;
        }
        else
        {
            // No tension on the handle visual, so handle visual and handle interactable are in the same location
            handleDisplacement = handleInteractableDisplacement;
            handleDistance = handleDisplacement.magnitude;
        }

        if (handleDistance < handleDisplacementTolerance)
        {
            handleVisualTransform.position = handleOriginPosition;
        }
    }

    private void ApplyHandleReturnForce()
    {
        // Returns handle to origin at a LINEAR RATE (this is not a gravitational pull which would result in a pendulum without drag)
        Vector3 ReturnForce = (handleOriginPosition - handleVisualTransform.position) * handleReturnForceCoefficient;
        handleVelocity = ReturnForce * Time.deltaTime;
    }

    // Testing function a la "wizard of Oz." Focussing on engine specific movement
    private void ApplyHandleOzForce()
    {
        if (!shouldApplyOzForceToHandle) return;

        Vector3 ozForce = Vector3.zero;

        // Apply a force to the handle to displace it (this moves engines which is helpful for movement debugging)
        if (shouldApplyOzForceToHandleP10_000_000) ozForce += ozForceToHandleP10_000_000 * ozForceFactor;
        if (shouldApplyOzForceToHandleN10_000_000) ozForce += ozForceToHandleN10_000_000 * ozForceFactor;
        if (shouldApplyOzForceToHandle000_P10_000) ozForce += ozForceToHandle000_P10_000 * ozForceFactor;
        if (shouldApplyOzForceToHandle000_N10_000) ozForce += ozForceToHandle000_N10_000 * ozForceFactor;
        if (shouldApplyOzForceToHandle000_000_P10) ozForce += ozForceToHandle000_000_P10 * ozForceFactor;
        if (shouldApplyOzForceToHandle000_000_N10) ozForce += ozForceToHandle000_000_N10 * ozForceFactor;
        handleVelocity += ozForce * Time.deltaTime;
    }
    private void ApplyHandleVibration()
    {
        // Apply an basenote of haptics to inform player they are holding the throttle
        handleInteractable.handAttached.hapticAction.Execute(0.0f, Time.deltaTime, 20.0f, 0.05f, handleInteractable.handAttached.handType);

        // Apply further haptics to inform user that they are struggling to push this throttle to its brink
        if (handleDistance > handleTensionRadiusMin)
        {
            float hapticAmplitudeHandleTensionMax = handleRadiusMax - handleTensionRadiusMin;
            float hapticAmplitudeHandleTension = (handleDistance - handleTensionRadiusMin) / hapticAmplitudeHandleTensionMax;
            handleInteractable.handAttached.hapticAction.Execute(0.0f, 0.2f, 20.0f, hapticAmplitudeHandleTension, handleInteractable.handAttached.handType);
        }
    }
}
