using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class Bleeder : MonoBehaviour
{

    public UnityEvent BleederAction;

    private Rigidbody rigidBody;

    private Vector3 currentRotation;
    private Vector3 currentPosition;
    private Vector3 prevPosition;
    private Vector3 startPosition;
    private Vector3 startRotation;
    private Vector3 parentPosition;
    private float maxDistance;
    private float maxAngle;
    private float parentPoint;
    private float distanceMoved;
    private float maxDip;
    private float parentStart;
    private float countdown;
    private bool alreadyClosed;
    private bool valveStatus;

    private TankStatus tankStatus;

    public GameObject parent;
    public GameObject child;
    public Lever tankLever;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        startRotation = transform.localEulerAngles;
        startPosition = transform.localPosition;
        prevPosition = startPosition;
        currentPosition = startPosition;
        currentRotation = startRotation;
        maxAngle = 90;
        parentPoint = (parent.transform.localPosition.x - (parent.transform.localScale.x * 1.5f));
        maxDistance = Mathf.Abs(parentPoint - startPosition.x);
        distanceMoved = 0;
        maxDip = parent.transform.localScale.z * 4 / 5;
        parentPosition = parent.transform.localPosition;
        parentStart = parentPosition.z;
        tankStatus = tankLever.GetComponent<TankStatus>();
        countdown = 5;
        alreadyClosed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tankStatus.IsTankOpen() && countdown > 0 && distanceMoved <= 0 && valveStatus)
        {
            countdown -= Time.deltaTime;
        }
        UpdateTransform();
        if (countdown <= 0 && distanceMoved >= (maxDistance - .05f) && !alreadyClosed)
        {
            alreadyClosed = true;
        }
        if (alreadyClosed && !valveStatus)
        {
            alreadyClosed = false;
            countdown = 5;
        }
    }

    public void PickUp()
    {
        rigidBody.isKinematic = false;
    }

    public void LetGo()
    {
        rigidBody.isKinematic = true;
    }

    void UpdateTransform()
    {
        currentPosition = transform.localPosition;
        currentPosition.y = startPosition.y;
        currentPosition.z = startPosition.z;
        if (currentPosition.x < startPosition.x)
        {
            currentPosition = startPosition;
        }
        else if (currentPosition.x > parentPoint)
        {
            currentPosition.x = parentPoint;
        }
        if (currentPosition.x > prevPosition.x)
        {
            distanceMoved += Mathf.Abs(prevPosition.x) - Mathf.Abs(currentPosition.x);
            currentRotation.y = (distanceMoved / maxDistance) * maxAngle;
            parentPosition.z = parentStart - (distanceMoved / maxDistance) * maxDip;
        }
        else if (currentPosition.x < prevPosition.x)
        {
            distanceMoved += Mathf.Abs(prevPosition.x) - Mathf.Abs(currentPosition.x);
            currentRotation.y = (distanceMoved / maxDistance) * maxAngle;
            parentPosition.z = parentStart - (distanceMoved / maxDistance) * maxDip;
        }
        rigidBody.transform.localPosition = currentPosition;
        parent.transform.localPosition = parentPosition;
        prevPosition = currentPosition;
        if (currentRotation.y < 0)
        {
            currentRotation.y = 0;
        }
        else if (currentRotation.y > 90)
        {
            currentRotation.y = 90;
        }
        currentRotation.x = startRotation.x;
        currentRotation.z = startRotation.z;
        transform.localRotation = Quaternion.Euler(startRotation);
        child.transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void SetValveStatus(bool newStatus)
    {
        valveStatus = newStatus;
    }

    public bool GetValveStatus()
    {
        return valveStatus;
    }

    public bool GetClosedStatus()
    {
        return alreadyClosed;
    }
}
