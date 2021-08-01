using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]

public class Switch : MonoBehaviour
{
    //List of functions that are called when the switch is turned on/off
    [SerializeField] private UnityEvent TurnOn;
    [SerializeField] private UnityEvent TurnOff;

    [SerializeField] private bool isOn = false;

    //is left/right hand hovering over that switch
    private bool leftHandHovering = false;
    private bool rightHandHovering = false;

    private Vector3 onRotation;
    private Vector3 offRotation;

    //Input for using an interface
    public SteamVR_Action_Boolean UseInteractionAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("UseInteraction");

    private void Start()
    {
        offRotation = transform.localEulerAngles;
        onRotation = offRotation;
        if (onRotation.z != 0)
            onRotation.z *= -1;
        else
            onRotation.y *= -1;
        setRotation();
    }

    private void Update()
    {
        if(leftHandHovering)
        {
            if (UseInteractionAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
                Flip();
        }
        if(rightHandHovering)
        {
            if (UseInteractionAction.GetStateDown(SteamVR_Input_Sources.RightHand))
                Flip();
        }
    }

    public void Flip()
    {
        isOn = !isOn;
        setRotation();
    }

    private void setRotation()
    {

        if (isOn)
        {
            TurnOn.Invoke();
            transform.localRotation = Quaternion.Euler(onRotation);
        }
        else
        {
            TurnOff.Invoke();
            transform.localRotation = Quaternion.Euler(offRotation);
        }
    }

    protected virtual void OnHandHoverBegin(Hand hand)
    {
        if (hand.handType == SteamVR_Input_Sources.LeftHand)
        {
            leftHandHovering = true;
        }
        else
        {
            rightHandHovering = true;
        }
    }

    protected virtual void OnHandHoverEnd(Hand hand)
    {
        if (hand.handType == SteamVR_Input_Sources.LeftHand)
        {
            leftHandHovering = false;
        }
        else
        {
            rightHandHovering = false;
        }
    }
}
