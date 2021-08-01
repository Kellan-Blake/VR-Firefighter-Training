/// <summary>
/// This class
/// </summary>

using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]

public class Button : MonoBehaviour
{
    //if left/right hands are hovering over the button
    private bool leftHandHovering = false;
    private bool rightHandHovering = false;

    private bool isButtonDown;
    private float timer = 1.0f;

    //Text attached to the object
    private Transform childText;

    //Stored positions to depress the button when pressed
    
    private Vector3 startPosition;
    private Vector3 pressedPosition;

    [Tooltip ("How long until the next button held event is invoked")]
    [SerializeField] float timeBeforeHeld = 0.25f;

    public UnityEvent ButtonPress;
    public UnityEvent ButtonHeld;
    public UnityEvent ButtonReleased;

    //Interaction action to use the button
    public SteamVR_Action_Boolean UseInteractionAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("UseInteraction");

    private void Start()
    {
        timer = timeBeforeHeld;
        startPosition = this.gameObject.transform.localPosition;
        pressedPosition = startPosition;
        pressedPosition.x += .005f;
        if (transform.childCount > 0)
        {
            childText = transform.GetChild(0);
        }
    }

    private void Update()
    {
        if (leftHandHovering)
        {
            if (UseInteractionAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
                buttonDown();
            else if (UseInteractionAction.GetStateUp(SteamVR_Input_Sources.LeftHand))
            {
                buttonUp();
            }
        }
        if (rightHandHovering)
        {
            if (UseInteractionAction.GetStateDown(SteamVR_Input_Sources.RightHand))
                buttonDown();
            else if (UseInteractionAction.GetStateUp(SteamVR_Input_Sources.RightHand))
            {
                buttonUp();
            }
        }

        if (isButtonDown)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = timeBeforeHeld;
                ButtonHeld.Invoke();
            }
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
        if (childText != null)
        {
            EnlargeText();
        }
    }

    protected virtual void OnHandHoverEnd(Hand hand)
    {
        if (hand.handType == SteamVR_Input_Sources.LeftHand)
        {
            leftHandHovering = false;
            isButtonDown = false;
        }
        else
        {
            rightHandHovering = false;
            isButtonDown = false;
        }
        if (childText != null)
        {
            DecreaseText();
        }
    }

    private void EnlargeText()
    {
        Vector3 childPosition = childText.localPosition;
        childText.localScale *= 3;
        childPosition.z *= 3;
        childPosition.x *= 2;
        childText.localPosition = childPosition;
    }

    private void DecreaseText()
    {
        Vector3 childPosition = childText.localPosition;
        childText.localScale /= 3;
        childPosition.z /= 3;
        childPosition.x /= 2;
        childText.localPosition = childPosition;
    }

    private void buttonDown()
    {
        if (!enabled)
            return;
        timer = timeBeforeHeld;
        isButtonDown = true;
        ButtonPress.Invoke();
        gameObject.transform.localPosition = pressedPosition;
    }

    private void buttonUp()
    {
        if (!enabled)
            return;
        isButtonDown = false;
        gameObject.transform.localPosition = startPosition;
        ButtonReleased.Invoke();
    }
}