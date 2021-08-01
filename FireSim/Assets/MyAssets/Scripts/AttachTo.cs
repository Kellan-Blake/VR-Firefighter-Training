using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class AttachTo : MonoBehaviour
{

    public UnityEvent OnAttachment;
    public UnityEvent OnDetachment;

    public GameObject attachObject;
    private Vector3 attachObjectPosition;
    private Quaternion attachRotation;
    private Rigidbody rigidBody;
    private bool attached;

    // Start is called before the first frame update
    void Start()
    {
        attachObjectPosition = attachObject.transform.position;
        rigidBody = GetComponent<Rigidbody>();
        attachRotation = attachObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckDistance())
        {
            Attach();
        }
        else if (attached && !CheckDistance())
        {
            Detach();
        }
    }

    private bool CheckDistance()
    {
        if (Vector3.Distance(attachObjectPosition, transform.position) < .5f)
        {
            return true;
        }
        return false;
    }

    private void Attach()
    {
        if (attached == false)
        {
            OnAttachment.Invoke();
        }
        Vector3 newPosition = attachObjectPosition;
        newPosition.x = attachObjectPosition.x - (attachObject.transform.localScale.y * 2);
        transform.position = newPosition;
        transform.rotation = attachRotation;
        attached = true;
    }

    private void Detach()
    {
        attached = false;
        OnDetachment.Invoke();
    }

    //used with throwable script to set movement
    public void LetGo()
    {
        rigidBody.isKinematic = true;
    }

    //used with throwable script to set movement
    public void PickUp()
    {
        rigidBody.isKinematic = false;
    }
}
