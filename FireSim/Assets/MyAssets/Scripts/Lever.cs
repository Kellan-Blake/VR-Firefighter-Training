using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HingeJoint))]

public class Lever : MonoBehaviour
{
    public UnityEvent OnLeverChange;

    [SerializeField] private Rigidbody childrb;

    private Quaternion startRot;
    private HingeJoint hinge;
    private Rigidbody rb;
    private float prevAngle = 0;
    private float angleValue;
    private float totalAngle;
    private float min;
    private bool setSpring = false;

    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.localRotation;
        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
        prevAngle = hinge.angle;
        min = hinge.limits.min;
        float max = hinge.limits.max;
        totalAngle = max - min;
        calculateValue();
        OnLeverChange.Invoke();

    }

    // Update is called once per frame
    void Update()
    {
        if(!setSpring)
        {
            if(hinge.angle <= hinge.spring.targetPosition)
            {
                setSpring = true;
                hinge.useSpring = false;
                childrb.isKinematic = true;
                rb.isKinematic = true;
            }
        }
        Vector3 rot = startRot.eulerAngles;
        rot.x = transform.localEulerAngles.x;
        transform.localRotation = Quaternion.Euler(rot);
        if (hinge.angle != prevAngle)
        {
            calculateValue();
            OnLeverChange.Invoke();
        }
    }

    public float GetAngleValue()
    {
        return angleValue;
    }

    //used with throwable script to set movement
    public void LetGo()
    {
        rb.isKinematic = true;
        calculateValue();
        OnLeverChange.Invoke();
    }

    //used with throwable script to set movement
    public void PickUp()
    {
        rb.isKinematic = false;
    }

    private void calculateValue()
    {
        prevAngle = hinge.angle;
        angleValue = (hinge.angle - min) / totalAngle;
    }

    public void SetAngleValue(float angle)
    {
        angleValue = (angle - min) / totalAngle;
    }
}