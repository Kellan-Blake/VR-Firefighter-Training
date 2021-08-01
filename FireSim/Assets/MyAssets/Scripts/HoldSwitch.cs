using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]

public class HoldSwitch : MonoBehaviour
{
    //List of functions that are called when the switch is turned on/off
    [SerializeField] private UnityEvent OnUp;
    [SerializeField] private UnityEvent OnDown;
    [SerializeField] private UnityEvent OnOff;

    [SerializeField] private SwitchState state = SwitchState.Off;

    public enum SwitchState
    {
        Up,
        Down,
        Off
    }

    //is left/right hand hovering over that switch
    private bool leftHandHovering = false;
    private bool rightHandHovering = false;


    private HingeJoint hinge;
    private JointSpring up;
    private JointSpring down;
    private JointSpring off;

    //Input for using an interface
    public SteamVR_Action_Boolean DownInteractionAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("UseInteraction");
    public SteamVR_Action_Boolean UpInteractionAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("SecondaryInteraction");

    private void Start()
    {
        hinge = GetComponent<HingeJoint>();
        hinge.useSpring = true;
        off = new JointSpring();
        off.targetPosition = 0;
        off.spring = 100;
        up = new JointSpring();
        up.targetPosition = -20;
        up.spring = 100;
        down = new JointSpring();
        down.targetPosition = 20;
        down.spring = 100;
        //state = SwitchState.Off;
        setRotation();
        //OnOff.Invoke();
    }

    private void Update()
    {
        if(!leftHandHovering && !rightHandHovering && state != SwitchState.Off)
        {
            state = SwitchState.Off;
            setRotation();
        }
        if (leftHandHovering)
        {
            if (UpInteractionAction.GetStateDown(SteamVR_Input_Sources.LeftHand) && state == SwitchState.Off)
            {
                state = SwitchState.Up;
                setRotation();
            }
            else if (DownInteractionAction.GetStateDown(SteamVR_Input_Sources.LeftHand) && state == SwitchState.Off)
            {
                state = SwitchState.Down;
                setRotation();
            }
            else if (DownInteractionAction.GetStateUp(SteamVR_Input_Sources.LeftHand) && state == SwitchState.Down)
            {
                state = SwitchState.Off;
                setRotation();
            }
            else if (UpInteractionAction.GetStateUp(SteamVR_Input_Sources.LeftHand) && state == SwitchState.Up)
            {
                state = SwitchState.Off;
                setRotation();
            }
        }
        if (rightHandHovering)
        {
            if (UpInteractionAction.GetStateDown(SteamVR_Input_Sources.RightHand) && state == SwitchState.Off)
            {
                state = SwitchState.Up;
                setRotation();
            }
            else if (DownInteractionAction.GetStateDown(SteamVR_Input_Sources.RightHand) && state == SwitchState.Off)
            {
                state = SwitchState.Down;
                setRotation();
            }
            else if (DownInteractionAction.GetStateUp(SteamVR_Input_Sources.RightHand) && state == SwitchState.Down)
            {
                state = SwitchState.Off;
                setRotation();
            }
            else if (UpInteractionAction.GetStateUp(SteamVR_Input_Sources.RightHand) && state == SwitchState.Up)
            {
                state = SwitchState.Off;
                setRotation();
            }
        }
    }

    private void setRotation()
    {

        switch(state)
        {
            case SwitchState.Up:
                hinge.spring = up;
                OnUp.Invoke();
                break;
            case SwitchState.Down:
                hinge.spring = down;
                OnDown.Invoke();
                break;
            case SwitchState.Off:
                hinge.spring = off;
                OnOff.Invoke();
                break;
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
