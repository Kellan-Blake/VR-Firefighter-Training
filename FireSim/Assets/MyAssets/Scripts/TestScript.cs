using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestScript : MonoBehaviour
{

    public GameObject digitalDisplay;
    public TextMeshPro dischargeNumber;
    public TextMeshPro intakeNumber;
    public Gauge intakeGauge;
    public Lever leverOne;
    public Gauge gaugeOne;
    public HandCrank handCrank;
    public Gauge crankGauge;

    private MasterDischarge masterDischarge;
    private bool idleTested;
    private bool rpmTested;
    private bool pressureTested;
    private bool dischargeTested;
    private bool baseIntakeTested;
    private bool leverTested;
    private bool crankTested;
    private bool increaseIntakeTested;
    private bool decreaseIntakeTested;
    private bool increaseDischargeTested;
    private bool decreaseDischargeTested;


    // Start is called before the first frame update
    void Start()
    {
        masterDischarge = digitalDisplay.GetComponent<MasterDischarge>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dischargeTested)
        {
            CheckDischarge();
        }
        if (!baseIntakeTested)
        {
            CheckIntake();
        }
        if (!increaseIntakeTested)
        {
            CheckIncreaseIntake();
        }
        if (!decreaseIntakeTested)
        {
            CheckDecreaseIntake();
        }
        if (!leverTested)
        {
            CheckLeverOne();
        }
        if (!crankTested)
        {
            CheckHandCrank();
        }
        if (masterDischarge.GetMode() == MasterDischarge.PanelMode.Pressure && !rpmTested)
        {
            CheckRPMMode();
        }
        if (masterDischarge.GetMode() == MasterDischarge.PanelMode.RPM && !pressureTested)
        {
            CheckPressureMode();
        }
    }

    void CheckDischarge()
    {
        masterDischarge.Preset();
        if (dischargeNumber.text == "0175" && masterDischarge.GetPressure().ToString("0000") == "0175")
        {
            Debug.Log("Discharge correct and preset working");
            dischargeTested = true;
        }
    }

    void CheckIntake()
    {
        intakeNumber.GetComponent<GetIntake>().CheckIntake();
        if (intakeNumber.text == "0100" && intakeGauge.GetOutputValue().ToString("0000") == "0100")
        {
            Debug.Log("Intake part 1 correct");
            intakeNumber.GetComponent<GetIntake>().IncreaseIntake(50);
            intakeNumber.GetComponent<GetIntake>().CheckIntake();
            baseIntakeTested = true;
            
        }
    }

    void CheckIncreaseIntake()
    {
        if (intakeNumber.text == "0150" && intakeGauge.GetOutputValue().ToString("0000") == "0150")
        {
            Debug.Log("Intake part 2 correct");
            increaseIntakeTested = true;
        }
    }

    void CheckLeverOne()
    {
        leverOne.SetAngleValue(6.5f);
        float testPSI = ((6.5f + 10) / 16.5f) * 175;
        leverOne.GetComponent<LeverDischarge>().UpdateAngle();
        if (testPSI == gaugeOne.GetOutputValue())
        {
            Debug.Log("Lever One Correct");
            leverTested = true;
        }
    }

    void CheckHandCrank()
    {
        handCrank.SetAngleValue(1152);
        float testPSI = (1152f / 2880f) * 175f;
        handCrank.GetComponent<CrankDischarge>().UpdateAngle();
        if (testPSI == crankGauge.GetOutputValue() && testPSI == 70)
        {
            Debug.Log("Hand crank correct");
            crankTested = true;
        }
    }

    void CheckIdle()
    {
        masterDischarge.Idle();
        if (dischargeNumber.text == "0020" && masterDischarge.GetPressure().ToString("0000") == "0020")
        {
            Debug.Log("idle setting working");
            idleTested = true;
        }
    }

    void CheckRPMMode()
    {
        masterDischarge.RPMButton();
        if (masterDischarge.GetMode() == MasterDischarge.PanelMode.RPM)
        {
            Debug.Log("RPM button works");
            rpmTested = true;
        }
    }

    void CheckPressureMode()
    {
        masterDischarge.PressureButton();
        if (masterDischarge.GetMode() == MasterDischarge.PanelMode.Pressure)
        {
            Debug.Log("Pressure button works");
            pressureTested = true;
        }
    }

    void CheckDecreaseIntake()
    {
        intakeNumber.GetComponent<GetIntake>().DecreaseIntake(50);
        intakeNumber.GetComponent<GetIntake>().CheckIntake();
        if (intakeNumber.text == "0100" && intakeGauge.GetOutputValue().ToString("0000") == "0100")
        {
            Debug.Log("Intake decrease correct");
            decreaseIntakeTested = true;
        }
    }
}
