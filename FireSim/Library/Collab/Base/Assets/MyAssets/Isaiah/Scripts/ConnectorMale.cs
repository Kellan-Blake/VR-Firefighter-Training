using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorMale : Connector
{
    //null if not connected to any connectors
    public ConnectorFemale connectedFemale;

    private void OnTriggerEnter(Collider other)
    {
        if (!interactable)
        {
            return;
        }

        if (other.gameObject.GetComponent("ConnectorFemale") != null)
        {
            connectedFemale = other.gameObject.GetComponent<ConnectorFemale>();

            //If the female is already connected to something or is part of a hose, can't connect this male
            if (connectedFemale.attached || connectedFemale.interactable || connectedFemale.size != this.size)
            {
                connectedFemale = null;
                return;
            }
            attached = true;
            connectedFemale.attached = true;
            connectedFemale.connectedMale = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!interactable || !connectedFemale)
        {
            return;
        }

        if (other.gameObject == connectedFemale.gameObject)
        {
            attached = false;
            connectedFemale.attached = false;
            connectedFemale.connectedMale = null;
            connectedFemale = null;
        }
    }

    private void Update()
    {
        if (!interactable)
        {
            return;
        }

        if (attached != wasAttached)
        {
            //If just snapping to the point, set transform to snap point
            if (attached)
            {
                SetTransform(connectedFemale.snapPoint);
            }
            //If not attached, set the psi/gpm to 0
            if (!attached)
            {
                PSI = 0;
                GPM = 0;
            }
        }
        //If released and still attached, set transform back to the snap point
        else if (attached && !grabbed && wasGrabbed)
        {
            SetTransform(connectedFemale.snapPoint);
        }
        if (prevPSI != PSI)
        {
            prevPSI = PSI;
            OnPSIChange.Invoke();
        }
        wasAttached = attached;
        wasGrabbed = grabbed;
    }
}
