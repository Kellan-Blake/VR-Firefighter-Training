using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStatus : MonoBehaviour
{
    [SerializeField] private ErrorChecks errorChecks;
    public Lever tankLever;

    private float currentAngle;
    private float defaultAngle;
    private bool tankOpen;
    private bool primed;
    private bool externalStatus;

    // Start is called before the first frame update
    void Start()
    {
        defaultAngle = 0;
        externalStatus = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentAngle = tankLever.GetAngleValue();
        if (currentAngle > defaultAngle && primed)
        {
            tankOpen = true;
        }
        else
        {
            tankOpen = false;
        }
    }

    public bool IsTankOpen()
    {
        return tankOpen;
    }

    public void setPrime(bool primeStatus)
    {
        if (currentAngle <= defaultAngle)
        {
            primed = primeStatus;
        }
    }

    public void SetExternal(bool newStatus)
    {
        externalStatus = newStatus;
        errorChecks.SetExternalStatus(newStatus);
    }

    public bool GetExternalStatus()
    {
        return externalStatus;
    }
}
