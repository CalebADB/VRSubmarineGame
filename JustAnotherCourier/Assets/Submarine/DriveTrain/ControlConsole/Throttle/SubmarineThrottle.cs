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
    // SubmarineEngine engine

    // Physics
    private Vector3 handleVelocity;
    private Vector3 handleVelocityReturn;
    private Vector3 handleVelocityNatural;

    private Vector3 handleDisplacement;
    private float handleDisplacementMagnitude;

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

    // OZ (simulated player interaction)
    [SerializeField]
    public bool shouldApplyOzForceToHandle = true;
    [SerializeField, Range(0, 10)]
    public float ozForceFactor = 1.0f;
    [SerializeField]
    public bool shouldApplyOzForceToHandleP10_000_000 = false;
    Vector3 OzForceToHandleP10_000_000 = new Vector3(10.0f, 0.0f, 0.0f);
    [SerializeField]
    public bool shouldApplyOzForceToHandleN10_000_000 = false;
    Vector3 OzForceToHandleN10_000_000 = new Vector3(-10.0f, 0.0f, 0.0f);
    [SerializeField]
    public bool shouldApplyOzForceToHandle000_P10_000 = false;
    Vector3 OzForceToHandle000_P10_000 = new Vector3(0.0f, 10.0f, 0.0f);
    [SerializeField]
    public bool shouldApplyOzForceToHandle000_N10_000 = false;
    Vector3 OzForceToHandle000_N10_000 = new Vector3(0.0f, -10.0f, 0.0f);
    [SerializeField]
    public bool shouldApplyOzForceToHandle000_000_P10 = false;
    Vector3 OzForceToHandle000_000_P10 = new Vector3(0.0f, 0.0f, 10.0f);
    [SerializeField]
    public bool shouldApplyOzForceToHandle000_000_N10 = false;
    Vector3 OzForceToHandle000_000_N10 = new Vector3(0.0f, 0.0f, -10.0f);


    // Start is called before the first frame update
    void Start()
    {
        handleOriginPosition = transform.position;
        
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        foreach (Transform childTransform in childTransforms)
        {
            Debug.Log(childTransform.gameObject.name);
            if (childTransform.gameObject.name == "HandleVisual") handleVisualTransform = childTransform;
            if (childTransform.gameObject.name == "HandleInteractable") handleInteractableTransform = childTransform;
        }

        Debug.Assert((handleVisualTransform != null), "handleVisualTransform is null");
        Debug.Assert((handleInteractableTransform != null), "handleInteractableTransform is null");

        handleInteractable = handleInteractableTransform.GetComponent<SubmarineThrottleHandleInteractable>();
    }

    void Update()
    {
        CalculateHandleDisplacement();

        if (handleInteractable.isHandAttached)
        {
            handleVisualTransform.position = handleOriginPosition + handleDisplacement;
        }
        else
        {
            ApplyHandleOzForce();
            ApplyHandleReturnForce();

            handleVisualTransform.position += handleVelocityReturn * Time.deltaTime;
            handleInteractableTransform.position = handleVisualTransform.position;
        }

        //engine.control(HandleDisplacement.normalized, handleDisplacementMagnitude / handleRadiusMax);
    }

    private void FixedUpdate()
    {
        if (handleInteractable.isHandAttached)
        {
            ApplyHandleVibration();
        }
    }

    private void CalculateHandleDisplacement()
    {
        Vector3 handleInteractableDisplacement = handleInteractableTransform.position - handleOriginPosition;
        float handleInteractableDisplacementMagnitude = handleInteractableDisplacement.magnitude;
        if (handleInteractableDisplacementMagnitude > handleTensionRadiusMin)
        {
            Vector3 handleInteractableDirection = handleInteractableDisplacement.normalized;
            handleDisplacement = (handleInteractableDirection * handleTensionRadiusMin) + (handleInteractableDirection * (handleInteractableDisplacementMagnitude - handleTensionRadiusMin) * handleTensionFactor);
            handleDisplacementMagnitude = handleDisplacement.magnitude;


            if (handleDisplacementMagnitude > handleRadiusMax)
            {
                handleDisplacement = handleDisplacement.normalized * handleRadiusMax;
            }
        }
        else
        {
            handleDisplacement = handleInteractableDisplacement;
            handleDisplacementMagnitude = handleDisplacement.magnitude;
        }

        if (handleDisplacementMagnitude < handleDisplacementTolerance)
        {
            handleVisualTransform.position = handleOriginPosition;
        }

    }

    private void ApplyHandleReturnForce()
    {
        Vector3 ReturnForce = (handleOriginPosition - handleVisualTransform.position) * handleReturnForceCoefficient;
        handleVelocityReturn = ReturnForce * Time.deltaTime;
    }

    private void ApplyHandleOzForce()
    {
        if (!shouldApplyOzForceToHandle) return;

        Vector3 OzForce = Vector3.zero;
        if (shouldApplyOzForceToHandleP10_000_000) OzForce += OzForceToHandleP10_000_000 * ozForceFactor;
        if (shouldApplyOzForceToHandleN10_000_000) OzForce += OzForceToHandleN10_000_000 * ozForceFactor;
        if (shouldApplyOzForceToHandle000_P10_000) OzForce += OzForceToHandle000_P10_000 * ozForceFactor;
        if (shouldApplyOzForceToHandle000_N10_000) OzForce += OzForceToHandle000_N10_000 * ozForceFactor;
        if (shouldApplyOzForceToHandle000_000_P10) OzForce += OzForceToHandle000_000_P10 * ozForceFactor;
        if (shouldApplyOzForceToHandle000_000_N10) OzForce += OzForceToHandle000_000_N10 * ozForceFactor;
        handleVelocityReturn += OzForce * Time.deltaTime;
    }
    private void ApplyHandleVibration()
    {
        handleInteractable.handAttached.hapticAction.Execute(0.0f, Time.deltaTime, 20.0f, 0.05f, handleInteractable.handAttached.handType);

        if (handleDisplacementMagnitude > handleTensionRadiusMin)
        {
            float hapticAmplitudeHandleTensionMax = handleRadiusMax - handleTensionRadiusMin;
            float hapticAmplitudeHandleTension = (handleDisplacementMagnitude - handleTensionRadiusMin) / hapticAmplitudeHandleTensionMax;
            handleInteractable.handAttached.hapticAction.Execute(0.0f, 0.2f, 20.0f, hapticAmplitudeHandleTension, handleInteractable.handAttached.handType);
        }
    }

    //public void attachEngine(Engine inEngine)
    //{
    //    engine = inEngine;
    //}

}
