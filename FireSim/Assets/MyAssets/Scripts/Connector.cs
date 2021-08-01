/// <summary>
/// 
/// </summary>

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public abstract class Connector : MonoBehaviour
{
    [Tooltip("Event to be called when PSI changes")]
    [SerializeField] protected UnityEvent OnPSIChange;

    [Tooltip("Pressure of the connector")]
    [SerializeField] public float PSI;

    [Tooltip("GPM of the water flowing through the connector")]
    [SerializeField] public float GPM;

    //Requires the connector to have the SteamVR throwable script
    [Tooltip("If connector is interactable, it will snap to other points")]
    [SerializeField] public bool interactable = true;

    [Tooltip("Transform interactable connectors will snap to")]
    [SerializeField] public Transform snapPoint;

    [Tooltip("Size of the connector. Connectors can only connect to similar sized connectors")]
    [SerializeField] public float size = 4f;

    //Used to tell if the interactable connector is currently grabbed
    protected bool grabbed = false;
    //grabbed of the previous frame
    protected bool wasGrabbed = false;

    protected Rigidbody rb;

    //used to check if the connector is currently attached to anything
    public bool attached = false;
    //attached of the previous frame
    protected bool wasAttached = false;

    protected float prevPSI = 0;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    public void Grab()
    {
        grabbed = true;
        rb.isKinematic = false;
    }

    public void LetGo()
    {
        grabbed = false;
        if(!attached)
            rb.isKinematic = false;
    }

    protected void SetTransform(Transform _t)
    {
        this.transform.position = new Vector3(_t.position.x, _t.position.y, _t.position.z);
        this.transform.rotation = Quaternion.Euler(_t.rotation.eulerAngles);
        rb.isKinematic = true;
    }
}
