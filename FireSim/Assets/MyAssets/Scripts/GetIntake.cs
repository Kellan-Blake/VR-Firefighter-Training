using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetIntake : MonoBehaviour
{

    [SerializeField] private ErrorChecks errorText;
    [SerializeField] private TankStatus tank;
    [SerializeField] private MasterIntake officerValve;
    [SerializeField] private MasterIntake driverValve;

    public Gauge leftGaugeOne;
    public Gauge leftGaugeTwo;
    public Gauge leftGaugeThree;
    public Gauge leftGaugeFour;

    public Gauge rightGaugeOne;
    public Gauge rightGaugeTwo;
    public Gauge rightGaugeThree;
    public Gauge rightGaugeFour;

    public Gauge intakeGauge;

    private Gauge[] gauges;
    private float currentIntake;
    private float internalIntake;
    private float officerIntake;
    private float driverIntake;
    private float discharge;
    private float[] gaugeValues;
    private TextMeshPro intakeText;
    private float displayIntake;

    private void Awake()
    {
        intakeText = GetComponent<TextMeshPro>();
        intakeText.text = "0100";
        currentIntake = 0;
        displayIntake = currentIntake;
        intakeGauge.SetInput(currentIntake / 600);
        gauges = new Gauge[8];
        gauges[0] = leftGaugeOne;
        gauges[1] = leftGaugeTwo;
        gauges[2] = leftGaugeThree;
        gauges[3] = leftGaugeFour;
        gauges[4] = rightGaugeOne;
        gauges[5] = rightGaugeTwo;
        gauges[6] = rightGaugeThree;
        gauges[7] = rightGaugeFour;
        gaugeValues = new float[8];
        for (int i = 0; i < 8; i++)
        {
            gaugeValues[i] = gauges[i].GetOutputValue();
        }
    }

    // Start is called before the first frame update
    void Update()
    {
        GetDischarges();
        UpdateIntake();
    }

    public void CheckIntake()
    {
        if (!tank.IsTankOpen() || !(officerValve.GetAngle() > 0 && officerValve.GetPSI() > 0) || !(driverValve.GetAngle() > 0 && driverValve.GetPSI() > 0))
        {
            return;
        }
        for (int i = 0; i < 8; i++)
        {
            if (gaugeValues[i] != gauges[i].GetOutputValue())
            {
                currentIntake += (gaugeValues[i] - gauges[i].GetOutputValue()) * 0.2f;
                if (currentIntake > 600)
                {
                    displayIntake = 600;
                    Debug.Log("Intake exceeded max");
                }
                else if (currentIntake <= 0)
                {
                    displayIntake = 0;
                }
                else
                {
                    displayIntake = currentIntake;
                }
                errorText.CheckIntake(currentIntake);
                gaugeValues[i] = gauges[i].GetOutputValue();
                intakeText.text = displayIntake.ToString("0000");
                intakeGauge.SetInput(displayIntake / 600);
            }
        }
    }

    public void SetTankIntake(float _intake)
    {
        internalIntake = _intake;
    }

    public void SetOfficerIntake(float _intake)
    {
        officerIntake = _intake;
    }

    public void SetDriverIntake(float _intake)
    {
        driverIntake = _intake;
    }

    private void UpdateIntake()
    {
        currentIntake = internalIntake + driverIntake + officerIntake - discharge;

        if (currentIntake > 600)
        {
            displayIntake = 600;
        }
        else if (currentIntake < 0)
        {
            displayIntake = 0;
        }
        else
        {
            displayIntake = currentIntake;
        }
        errorText.CheckIntake(currentIntake);
        intakeText.text = displayIntake.ToString("0000");
        intakeGauge.SetInput(displayIntake / 600);
    }

    private void GetDischarges()
    {
        float _discharge = 0;

        for(int i = 0; i < 8; i++)
        {
            _discharge += gauges[i].GetOutputValue();
            gaugeValues[i] = gauges[i].GetOutputValue();
        }

        discharge = _discharge * .2f;
    }

    public bool IncreaseIntake(float intakeAmount)
    {
        if (!tank.IsTankOpen() || (officerValve.GetAngle() > 0 && officerValve.GetPSI() > 0) || (driverValve.GetAngle() > 0 && driverValve.GetPSI() > 0))
        {
            return false;
        }
        currentIntake += intakeAmount;
        if (currentIntake > 600)
        {
            displayIntake = 600;
            return false;
        }
        else if(currentIntake < 0)
        {
            displayIntake = 0;
            return false;
        }
        else
        {
            displayIntake = currentIntake;
        }
        intakeText.text = displayIntake.ToString("0000");
        intakeGauge.SetInput(displayIntake / 600);
        return true;
    }

    public bool DecreaseIntake(float intakeAmount)
    {
        if (!tank.IsTankOpen() || (officerValve.GetAngle() > 0 && officerValve.GetPSI() > 0) || (driverValve.GetAngle() > 0 && driverValve.GetPSI() > 0))
        {
            return false;
        }
        currentIntake -= intakeAmount;
        if (currentIntake < 0)
        {
            displayIntake = 0;
            return false;
        }
        else
        {
            displayIntake = currentIntake;
        }
        intakeText.text = displayIntake.ToString("0000");
        intakeGauge.SetInput(displayIntake / 600);
        return true;
    }

    public float GetCurrentIntake()
    {
        return displayIntake;
    }
}
