using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Collider))]
[RequireComponent(typeof (Rigidbody))]

public class SnapTo : MonoBehaviour
{
    [Tooltip("Triggers snap when collides with target")]
    [SerializeField] private GameObject target;

    [Tooltip("Transform when snapped")]
    [SerializeField] private Transform targetPosition;

    private bool attached = false;
    private bool wasAttached = false;
    private bool grabbed = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
        {
            attached = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            attached = false;
        }
    }

    private void Update()
    {
        if(attached != wasAttached)
        {
            wasAttached = attached;
            if(attached)
            {
                this.transform.position = new Vector3(targetPosition.position.x, targetPosition.position.y, targetPosition.position.z);
                this.transform.rotation = Quaternion.Euler(targetPosition.rotation.eulerAngles);
                this.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else if(attached && !grabbed)
        {
            this.transform.position = new Vector3(targetPosition.position.x, targetPosition.position.y, targetPosition.position.z);
            this.transform.rotation = Quaternion.Euler(targetPosition.rotation.eulerAngles);
        }
    }

    public void Grab()
    {
        grabbed = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void LetGo()
    {
        grabbed = false;
    }
}
