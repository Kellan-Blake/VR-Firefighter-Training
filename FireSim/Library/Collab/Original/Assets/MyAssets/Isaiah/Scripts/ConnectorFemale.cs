using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorFemale : Connector
{
    //null if not connected to any connectors
    public ConnectorMale connectedMale;
    [SerializeField] private ValveIntake intakeValve;

    private void OnTriggerEnter(Collider other)
    {
        if (!interactable)
        {
            return;
        }

        if(other.gameObject.GetComponent("ConnectorMale") != null)
        {
            connectedMale = other.gameObject.GetComponent<ConnectorMale>();

            //If the male is already connected to something or part of a hose, can't connect this female
            if (connectedMale.attached || connectedMale.interactable || connectedMale.size != this.size)
            {
                connectedMale = null;
                return;
            }
            attached = true;
            connectedMale.attached = true;
            connectedMale.connectedFemale = this;
            intakeValve.FemaleConnected();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!interactable || !connectedMale)
        {
            return;
        }

        if(other.gameObject == connectedMale.gameObject)
        {
            attached = false;
            connectedMale.connectedFemale = null;
            connectedMale.attached = false;
            connectedMale = null;
            intakeValve.FemaleDetached();
        }
    }

    private void Update()
    {
        if(!interactable)
        {
            if(attached != wasAttached)
            {
                if(attached)
                {
                    PSI = connectedMale.PSI;
                    GPM = connectedMale.GPM;
                }
                else if(!attached)
                {
                    PSI = 0;
                    GPM = 0;
                }
                wasAttached = attached;

            }
            if (prevPSI != PSI)
            {
                prevPSI = PSI;
                OnPSIChange.Invoke();
            }
            return;
        }

        if (attached != wasAttached)
        {
            //If just snapping to the point, set transform to snap point
            if (attached)
            {
                SetTransform(connectedMale.snapPoint);
                PSI = connectedMale.PSI;
                GPM = connectedMale.GPM;
            }
            //If not attached, set the psi/gpm to 0
            if(!attached)
            {
                PSI = 0;
                GPM = 0;
            }
        }
        //If released and still attached, set transform back to the snap point
        else if (attached && !grabbed && wasGrabbed)
        {
            SetTransform(connectedMale.snapPoint);
        }

        if(prevPSI != PSI)
        {
            prevPSI = PSI;
            OnPSIChange.Invoke();
        }
        wasAttached = attached;
        wasGrabbed = grabbed;
    }
}
