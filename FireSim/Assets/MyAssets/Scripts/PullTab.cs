using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Pull tabs are VR Interactables that trigger events when pulled back all the way and events
/// when the tab is reset back to its starting position.
/// </summary>

public class PullTab : MonoBehaviour
{
    #region Instance Variables
    [Tooltip("Invoked when tab is pulled back all the way")]
    public UnityEvent HandlePulled;
    [Tooltip("Invoked after HandlePulled has been invoked and tab is reset back to starting position")]
    public UnityEvent HandleReset;

    [Tooltip("If true, the HandlePulled event will be invoked as long as the handle is pulleds")]
    public bool HoldTab = false;

    
    private bool alreadyPulled = false;

    private Vector3 pulledPosition;
    private Vector3 lockedPosition;
    private Vector3 startPosition;

    private Quaternion startRotation;

    private Rigidbody rigidBody;
    #endregion

    #region Unity Methods
    void Start()
    {
        Init();
    }

    void Update()
    {
        SetPosition();
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Sets the Instance Variables
    /// </summary>
    private void Init()
    {
        rigidBody = GetComponent<Rigidbody>();
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        pulledPosition = startPosition;
        lockedPosition = startPosition;
        lockedPosition.x -= 1.6f;
    }

    /// <summary>
    /// Locks the position of the pull tab and checks to invoke events
    /// </summary>
    private void SetPosition()
    {
        pulledPosition = transform.localPosition;
        pulledPosition.y = startPosition.y;
        pulledPosition.z = startPosition.z;

        if (pulledPosition.x > startPosition.x)
        {
            pulledPosition = startPosition;
            alreadyPulled = false;
            HandleReset.Invoke();
        }

        else if (pulledPosition.x <= (startPosition.x - 1.6f))
        {
            pulledPosition = lockedPosition;
            if (!alreadyPulled || HoldTab)
            {
                alreadyPulled = true;
                HandlePulled.Invoke();
            }
        }

        rigidBody.transform.localPosition = pulledPosition;
        transform.localRotation = startRotation;
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Call when user grabs the pull tab
    /// </summary>
    public void Grab()
    {
        rigidBody.isKinematic = false;
    }

    /// <summary>
    /// Call when the user lets go of the pull tab
    /// </summary>
    public void LetGo()
    {
        rigidBody.isKinematic = true;
        rigidBody.transform.localPosition = startPosition;
        alreadyPulled = false;
        HandleReset.Invoke();
    }
    #endregion
}
