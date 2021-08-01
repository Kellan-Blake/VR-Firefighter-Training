using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorChecks : MonoBehaviour
{
    //[SerializeField] private TankStatus

    private TextMeshPro errorMessage;
    private string dischargeWarning;
    private string intakeWarning;
    private float correctDischarge;
    private string GPMWarning;
    private string tankWarning;
    private bool externalStatus;

    // Start is called before the first frame update
    void Start()
    {
        errorMessage = GetComponent<TextMeshPro>();
        errorMessage.text = "";
        dischargeWarning = "WARNING: Pressure too high for current scenario";
        intakeWarning = "WARNING: Discharge too high for water intake";
        GPMWarning = "WARNING: Discharge gallons per minute too high for only tank";
        tankWarning = "WARNING: Internal tank empty";
        externalStatus = false;
    }



    public void CheckDischarge(float currentDischarge)
    {
        if (currentDischarge >= (correctDischarge + 100))
        {
            errorMessage.text = dischargeWarning;
        }
        else if (currentDischarge < (correctDischarge + 100) && errorMessage.text == dischargeWarning)
        {
            errorMessage.text = "";
        }
    }

    public void CheckIntake(float currentIntake)
    {
        if (currentIntake < 0)
        {
            errorMessage.text = intakeWarning;
        }
        else if (currentIntake >= 0 && errorMessage.text == intakeWarning)
        {
            errorMessage.text = "";
        }
    }

    public void CheckGPM(float dischargeGPM)
    {
        if (dischargeGPM > 300 && !externalStatus)
        {
            errorMessage.text = GPMWarning;
        }
        else if (dischargeGPM <= 300 || externalStatus && errorMessage.text == GPMWarning)
        {
            errorMessage.text = "";
        }
    }

    public void CheckTankVolume(float tankVolume)
    {
        if (tankVolume <= 0)
        {
            errorMessage.text = tankWarning;
        }
        else if (tankVolume > 0 && errorMessage.text == tankWarning)
        {
            errorMessage.text = "";
        }
    }

    public void SetCorrect(float newCorrect)
    {
        correctDischarge = newCorrect;
    }

    public void SetExternalStatus(bool newStatus)
    {
        externalStatus = newStatus;
    }
}
