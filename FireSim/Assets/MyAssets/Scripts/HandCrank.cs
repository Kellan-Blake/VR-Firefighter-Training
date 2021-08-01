using UnityEngine;
using UnityEngine.Events;

public class HandCrank : MonoBehaviour
{
    //event to be called when the angle changes
    [SerializeField] private UnityEvent OnCrankUpdate;

    //rigidbodies to manipulate
    private Rigidbody rb;
    [SerializeField] private Rigidbody parent;

    //this objects transform
    private Vector3 startPosition;
    private Vector3 startRotation;

    //parents objects transform
    private Vector3 parentStartPosition;
    private Quaternion parentOldRotation;
    private Quaternion parentMinRotation;
    private Quaternion parentMaxRotation;

    //Max Rotation in degress
    [SerializeField] private float maxRotation = 720;

    //current angle
    private float angle = 0;

    //0-1 value of the angle
    private float angleValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.localPosition;
        startRotation = transform.localEulerAngles;
        parentStartPosition = parent.transform.localPosition;
        parentOldRotation = parent.transform.localRotation;
        setUpMinMax();
        calculateAngle();
        OnCrankUpdate.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition != startPosition)
        {
            transform.localPosition = startPosition;
        }
        if(parent.transform.localPosition != parentStartPosition)
        {
            parent.transform.localPosition = parentStartPosition;
        }
        transform.localRotation = Quaternion.Euler(0, startRotation.y, 0);
        if(parent.transform.localRotation.eulerAngles.y != parentOldRotation.eulerAngles.y)
        {
            calculateAngle();
        }
    }

    public float GetAngleValue()
    {
        return angleValue;
    }

    public void Grab()
    {
        rb.isKinematic = false;
        parent.isKinematic = false;
    }

    public void LetGo()
    {
        rb.isKinematic = true;
        parent.isKinematic = true;
        parent.velocity = default (Vector3);
        calculateAngle();
    }

    private void setUpMinMax()
    {
        parentMinRotation = parent.transform.localRotation;
        Vector3 max = parentMinRotation.eulerAngles;
        max.y += maxRotation;
        parentMaxRotation = Quaternion.Euler(max);
    }

    private void calculateAngle()
    {
        int increase = 1;
        Quaternion rot = parent.transform.localRotation;

        //Finished one full revolution. Angle increased
        if (rot.eulerAngles.y < 90 && parentOldRotation.eulerAngles.y > 270)
            increase = 1;
        //Finished one full revolution. Angle decreased
        else if (rot.eulerAngles.y > 270 && parentOldRotation.eulerAngles.y < 90)
            increase = -1;
        //Angled decreased
        else if (rot.eulerAngles.y < parentOldRotation.eulerAngles.y)
            increase = -1;

        //Get angle between old and new rotation
        float deltaAngle = Quaternion.Angle(rot, parentOldRotation) * increase;

        //Set previous rot = current rot
        parentOldRotation = rot;

        //Get new angle 
        float newAngle = deltaAngle + angle;

        //Check if new angle is greater or less than min/max
        if(newAngle > maxRotation + 1)
        {
            //If greater than max, then calculate the difference to set to max angle and set rotation to max
            newAngle = maxRotation;
            parent.transform.localRotation = parentMaxRotation;
            parentOldRotation = parent.transform.localRotation;
        }

        else if(newAngle < -1)
        {
            //If less than min (0), then calculate the difference to set to min angle and set rotation to 0
            newAngle = 0;
            parent.transform.localRotation = parentMinRotation;
            parentOldRotation = parent.transform.localRotation;
        }

        //if new angle is not previous angle
        if(newAngle != angle)
        {   
            //Set new angle, calculate value, and update
            angle = newAngle;
            calculateAngleValue();
            OnCrankUpdate.Invoke();
        }
    }

    private void calculateAngleValue()
    {
        angleValue = angle / maxRotation;
    }

    public void SetAngleValue(float angle)
    {
        angleValue = angle / maxRotation;
    }
}
