using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class ExternalDirector : Director
{

    private float correctCountdown = 15;
    private float targetOffset = 15;
    private float openLines = 0;
    private bool resultsCorrect = false;
    private bool testOver = false;
    private List<Discharge> discharges;
    private string passMessage;
    private string failMessage;
    private TextMeshPro passText;
    private bool testStarted;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

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
                if (correctCountdown <= 0)
                {
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
        if (!CheckExternal())
        {
            resultsCorrect = false;
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
        if (openLines < 1)
        {
            return false;
        }
        openLines = 0;
        return true;
    }

    private bool CheckDischarge()
    {
        foreach (Discharge _discharge in discharges)
        {
            switch(_discharge.GetIndex())
            {
                case 1:
                    if(!between(_discharge.GetPSI(), CorrectPSI[0]))
                    {
                        return false;
                    }
                    break;
                case 2:
                    if (!between(_discharge.GetPSI(), CorrectPSI[1]))
                    {
                        return false;
                    }
                    break;
                case 3:
                    if (!between(_discharge.GetPSI(), CorrectPSI[2]))
                    {
                        return false;
                    }
                    break;
                case 4:
                    if (!between(_discharge.GetPSI(), CorrectPSI[3]))
                    {
                        return false;
                    }
                    break;
                case 5:
                    if (!between(_discharge.GetPSI(), CorrectPSI[4]))
                    {
                        return false;
                    }
                    break;
                case 6:
                    if (!between(_discharge.GetPSI(), CorrectPSI[5]))
                    {
                        return false;
                    }
                    break;
                case 7:
                    if (!between(_discharge.GetPSI(), CorrectPSI[6]))
                    {
                        return false;
                    }
                    break;
                case 8:
                    if (!between(_discharge.GetPSI(), CorrectPSI[7]))
                    {
                        return false;
                    }
                    break;
                case 9:
                    if (!between(_discharge.GetPSI(), CorrectPSI[8]))
                    {
                        return false;
                    }
                    break;
                default:
                    Debug.LogError("If this is reached, we should switch the List to a dictionary");
                    return false;
                    break;
            }
        }
        return true;
    }

    private bool CheckIntake()
    {
        if (intake.GetCurrentIntake() > 20)
        {
            testStarted = true;
        }
        if (!testStarted)
        {
            return true;
        }
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

    private bool between(float _check, float _target)
    {
        if(_check < _target - 15 || _check > _target + 15)
        {
            return false;
        }
        return true;
    }
}
