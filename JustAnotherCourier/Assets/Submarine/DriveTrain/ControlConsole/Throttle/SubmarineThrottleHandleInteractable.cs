using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using static Valve.VR.InteractionSystem.Hand;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class SubmarineThrottleHandleInteractable : MonoBehaviour
{
    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);

    private Interactable interactable;

    public bool isHandAttached = false;
    public Hand handAttached = null;

    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    // Managing hover and hand attachment state
    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting(GrabTypes.Grip);
        bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            // Lock hover on this object
            hand.HoverLock(interactable);

            // Attach to hand
            hand.AttachObject(gameObject, startingGrabType, attachmentFlags);

            // Update state
            isHandAttached = true;
            handAttached = hand;
        }
        else if (isGrabEnding)
        {
            // Detach from hand
            hand.DetachObject(gameObject);

            // Unlock hover on this object
            hand.HoverUnlock(interactable);

            // Update state
            isHandAttached = false;
            handAttached = null;
        }
    }
}
