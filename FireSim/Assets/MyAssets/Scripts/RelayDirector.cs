using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RelayDirector : Director
{

    [SerializeField] private MasterDischarge relayDischarge;
    [SerializeField] private GetIntake relayIntake;
    [SerializeField] private Bleeder relayOfficerBleeder;
    [SerializeField] private Bleeder relayDriverBleeder;
    [SerializeField] private MasterIntake relayOfficerValve;
    [SerializeField] private MasterIntake relayDriverValve;

    private float correctCountdown = 15;
    private float targetOffset = 15;
    private float openLines = 0;
    private bool resultsCorrect = false;
    private bool testOver = false;
    private List<Discharge> discharges;
    private string passMessage;
    private string failMessage;
    private TextMeshPro passText;

    // Start is called before the first frame update
    void Start()
    {
        buttonCanvas.enabled = false;
        passText = GetComponentInChildren<TextMeshPro>();
        discharges = masterDischarge.GetDischarges();
        passMessage = "Test Passed";
        failMessage = "Test Failed";
        passText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!testOver)
        {
            CheckResults();
            if (resultsCorrect)
            {
                correctCountdown -= Time.deltaTime;
                Debug.Log(correctCountdown);
                if (correctCountdown <= 0)
                {
                    Debug.Log("passed");
                    Pass();
                }
            }
            else
            {
                correctCountdown = 15;
            }
        }
    }

    protected override void CheckResults()
    {
        if (!CheckLevers())
        {
            resultsCorrect = false;
            return;
        }
        if (!CheckDischarge())
        {
            resultsCorrect = false;
            return;
        }
        if (!CheckIntake())
        {
            resultsCorrect = false;
            Fail();
            return;
        }
        if (!CheckExternal())
        {
            resultsCorrect = false;
            Fail();
            return;
        }
        if (!CheckRelay())
        {
            resultsCorrect = false;
            Fail();
            return;
        }
        resultsCorrect = true;
    }

    private bool CheckLevers()
    {
        foreach (Discharge d in discharges)
        {
            if (d.GetDischarge() > 0)
            {
                openLines++;
            }
        }
        if (openLines != 1)
        {
            return false;
        }
        openLines = 0;
        return true;
    }

    private bool CheckDischarge()
    {
        if (masterDischarge.GetPressure() > (correctDischarge + targetOffset) || masterDischarge.GetPressure() < (correctDischarge - targetOffset))
        {
            return false;
        }
        return true;
    }

    private bool CheckIntake()
    {
        if (intake.GetCurrentIntake() < 20)
        {
            return false;
        }
        return true;
    }

    private bool CheckExternal()
    {
        if (officerValve.GetAngle() > 0 && officerValve.GetPSI() > 0)
        {
            if (officerBleeder.GetClosedStatus())
            {
                return true;
            }
        }
        if (driverValve.GetAngle() > 0 && driverValve.GetPSI() > 0)
        {
            if (driverBleeder.GetClosedStatus())
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckRelay()
    {
        if (relayOfficerValve.GetAngle() > 0 && relayOfficerValve.GetPSI() > 0)
        {
            if (relayOfficerBleeder.GetClosedStatus())
            {
                return true;
            }
        }
        if (relayDriverValve.GetAngle() > 0 && relayDriverValve.GetPSI() > 0)
        {
            if (relayDriverBleeder.GetClosedStatus())
            {
                return true;
            }
        }
        return false;
    }

    private void Pass()
    {
        testOver = true;
        passText.text = passMessage;
        buttonCanvas.enabled = true;
    }

    private void Fail()
    {
        testOver = true;
        passText.text = failMessage;
        buttonCanvas.enabled = true;
    }
}
