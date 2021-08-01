/// <summary>
/// Governer of the pump panel. Used to set either the target rpm or psi for the pump to use.
/// RPM Mode will keep the rpm of the engine the same. PSI mode will set the
/// max psi that an individual discharge can be set to.
/// </summary>

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Valve.Newtonsoft.Json.Bson;

public class MasterDischarge : MonoBehaviour
{
    #region Instance Variables
    //What to call when the pressure/rpm is changed
    public UnityEvent OnPressureChange;

    [Tooltip("When true, the pump will be shut down and can't be turned on")]
    public bool EmergencyOverride = false;

    //Max pressure that the pump panel can handle
    [Range(1, 600)]
    [SerializeField] private int maxPressure = 600;
    //Max rpm that the engine can handle
    [SerializeField] private int maxRPM = 3000;
    //What the PSI can change by each second
    [SerializeField] private int deltaPSI = 50;
    //What the RPM can change by each second
    [SerializeField] private int deltaRPM = 150;

    //conversion of psi to rpm (May not be accurate. Investigate further)
    [SerializeField] private float psiToRPM = 2;

    //Is this even still used?
    [SerializeField] private float correctPressure;

    //Gauge for the psi of the pump
    [SerializeField] private Gauge pumpDischarge;
    //Gauge for the Intake of the pump
    [SerializeField] private Gauge pumpIntake;
    //Gauge for the rpm of the engine
    [SerializeField] private Gauge rpmGauge;

    //Light that mode is pressure
    [SerializeField] private GameObject pressureLight;
    //Light that mode is rpm
    [SerializeField] private GameObject rpmLight;

    //Text that displays what the current rpm and psi is
    [SerializeField] private TextMeshPro psiText;
    [SerializeField] private TextMeshPro rpmText;

    //List of all the individual discharge lines
    [SerializeField] private List<Discharge> discharges = new List<Discharge>();

    //Checks for errors and display warnings
    [SerializeField] private ErrorChecks errorText;

    //Audio of the engine
    [SerializeField] private EngineAudio engine;

    //Intake of the pump
    [SerializeField] private GetIntake intakePressure;

    //Current rpm and psi of the pump
    private float currentRPM = 0;
    private float currentPressure = 0;
    //Target rpm and psi of the pump
    private float targetPSI = 0;
    private float targetRPM = 0;
    //Total pressure of all individual discharges
    private float totalPSI = 0;
    //Total angle of all the individual discharges (0 = all closed, +1 for each fully opened discharge
    private float totalAngle = 0;
    //Total angle last frame
    private float previousAngle = 0;

    //Current intake
    private float currentIntake = 0;
    //Current intake last frame
    private float prevIntake = 0;

    //If the pump on/off
    private bool pumpEngaged = false;

    //Current mode of the panel
    private PanelMode currentMode;
    //Previous mode of the panel when switching to idle or preset
    private PanelMode previousMode;
    #endregion

    #region Unity Methods
    private void Start()
    {
        //Should change so that max is an instance variable
        engine.setMax(3000);
        //Make sure engine audio is turned at start
        engine.gameObject.SetActive(false);
        //pump is false
        pumpEngaged = false;
        //Is this still needed, look at later
        errorText.SetCorrect(correctPressure);
        //panel is off
        currentMode = PanelMode.Off;
    }

    private void Update()
    {
        //Update intake
        currentIntake = intakePressure.GetCurrentIntake();
        UpdateDischarge();
        CheckOverride();
    }
    #endregion

    #region Public Methods
    
    /// <summary>
    /// Adds an individual discharge line to the list of all discharges.
    /// </summary>
    /// <param name="_discharge">The individual discharge to add.</param>
    public void AddDischarge(Discharge _discharge)
    {
        discharges.Add(_discharge);
    }

    /// <summary>
    /// Sets the Emergency Override
    /// </summary>
    /// <param name="_on">New bool for override</param>
    public void SetEmergency(bool _on)
    {
        EmergencyOverride = _on;
    }

    /// <summary>
    /// Turns on the pump panel and starts the audio
    /// </summary>
    public void TurnOn()
    {
        if (pumpEngaged || EmergencyOverride)
        {
            return;
        }
        //Start in pressure mode
        currentMode = PanelMode.Pressure;
        previousMode = PanelMode.Pressure;

        //Turn on audio
        engine.gameObject.SetActive(true);
        pumpEngaged = true;
        targetPSI = 0;
        engine.setRPM(0);
        currentRPM = 0;
        psiText.text = targetPSI.ToString("0000");
    }

    /// <summary>
    /// Start the turn off sequence for the pump panel
    /// </summary>
    public void TurnOff()
    {
        if (!pumpEngaged)
        {
            return;
        }

        pumpEngaged = false;
        targetRPM = 0;
        currentMode = PanelMode.RPM;
        previousMode = currentMode;
        rpmText.text = targetRPM.ToString("0000");
    }

    /// <summary>
    /// The different modes that the panel can be in
    /// </summary>
    public enum PanelMode
    {
        Pressure,
        RPM,
        Idle,
        Preset,
        Off
    }

    /// <summary>
    /// Used by other classes to change the rpm/psi of the pump. 
    /// </summary>
    /// <param name="_delta">Unscaled amount to change the pump by.</param>
    public void Increase(int _delta)
    {
        //Switches on the mode of the pump to either change the target psi
        //or the target rpm of the panel
        switch(currentMode)
        {
            case PanelMode.Pressure:
                IncreasePressure(_delta);
                break;
            case PanelMode.RPM:
                IncreaseRPM(_delta * 15);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Idles the pump
    /// </summary>
    //Need to change to set the rpm to 0 instead of psi
    public void Idle()
    {
        if (currentMode == PanelMode.Idle || !pumpEngaged || currentMode == PanelMode.Preset || currentMode == PanelMode.Off)
            return;
        //Unnecessary if check, always true if this point if reached
        if (currentMode == PanelMode.RPM || currentMode == PanelMode.Pressure)
            previousMode = currentMode;
        currentMode = PanelMode.Idle;
        targetPSI = 0;
        psiText.text = targetPSI.ToString("0000");
        errorText.CheckDischarge(targetPSI);
    }

    /// <summary>
    /// Sets the pump to the preset mode
    /// </summary>
    public void Preset()
    {
        if (currentMode == PanelMode.Preset || !pumpEngaged || currentMode == PanelMode.Idle || currentMode == PanelMode.Off)
            return;
        //Unnecessary if check, always true if this point if reached
        if (currentMode == PanelMode.RPM || currentMode == PanelMode.Pressure)
            previousMode = currentMode;
        currentMode = PanelMode.Preset;
        //Preset PSI for Engine 2 in the SFD
        targetPSI = 175;
        psiText.text = targetPSI.ToString("0000");
        errorText.CheckDischarge(targetPSI);
    }

    /// <summary>
    /// Returns the current pressure of the pump
    /// </summary>
    /// <returns>Current Pressure</returns>
    public float GetPressure()
    {
        return currentPressure;
    }

    /// <summary>
    /// Returns the total discharge of all the discharges
    /// </summary>
    /// <returns>TotalPSI: Sum of all the individual discharges in PSI</returns>
    public float GetTotalDischarge()
    {
        return totalPSI;
    }

    /// <summary>
    /// Returns a list of all individual discharges
    /// </summary>
    /// <returns>List of all discharges</returns>
    public List<Discharge> GetDischarges()
    {
        return discharges;
    }

    /// <summary>
    /// Sets the mode of the pump to change the RPM
    /// </summary>
    public void RPMButton()
    {
        if (currentMode == PanelMode.Preset || !pumpEngaged || currentMode == PanelMode.Idle || currentMode == PanelMode.Off)
            return;
        //Unnecessary if check, always true if this point if reached
        if (currentMode == PanelMode.RPM || currentMode == PanelMode.Pressure)
            previousMode = currentMode;
        currentMode = PanelMode.RPM;
        targetRPM = currentRPM;
    }

    /// <summary>
    /// Sets the mode of the pump to change the PSI
    /// </summary>
    public void PressureButton()
    {
        if (currentMode == PanelMode.Preset || !pumpEngaged || currentMode == PanelMode.Idle || currentMode == PanelMode.Off)
            return;
        //Unnecessary if check, always true if this point if reached
        if (currentMode == PanelMode.RPM || currentMode == PanelMode.Pressure)
            previousMode = currentMode;
        currentMode = PanelMode.Pressure;
        targetPSI = currentPressure;
    }

    /// <summary>
    /// Returns the current mode of the pump panel
    /// </summary>
    /// <returns>Pressure, RPM, Idle, Off, Preset</returns>
    public PanelMode GetMode()
    {
        return currentMode;
    }

    /// <summary>
    /// Increases/Decreases the total PSI from all the individual discharges
    /// </summary>
    /// <param name="_delta">Delta PSI of the individual discharge being changed</param>
    public void IncreaseTotalPSI(float _delta)
    {
        totalPSI += _delta;
        switch (currentMode)
        {
            //Only change the audio if in the Pressure mode, otherwise pressure should stay the same
            //During Idle, and Preset, is the audio already handled?
            case PanelMode.Pressure:
                currentRPM = ((currentPressure - currentIntake) * psiToRPM * (7f / 8f)) + (totalPSI * psiToRPM * (2f / 8f));
                if(currentRPM < 0)
                {
                    currentRPM = 0;
                }
                rpmText.text = currentRPM.ToString("0000");
                engine.setRPM(currentRPM);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Increases/Decreases the total angle of all the individual discharges
    /// </summary>
    /// <param name="_delta">Delta angle of the discharge being changed</param>
    public void IncreaseTotalAngle(float _delta)
    {
        //Each discharge should be +1 to the total angle
        totalAngle += _delta;
        if(totalAngle < 0)
        {
            totalAngle = 0;
        }
        //Discharge.Count is the max total angle
        else if(totalAngle > discharges.Count)
        {
            totalAngle = discharges.Count;
        }
    }

    public bool PumpOn()
    {
        return pumpEngaged;
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Checks for the emergency override
    /// </summary>
    private void CheckOverride()
    {
        if(EmergencyOverride && pumpEngaged)
        {
            TurnOff();
        }
    }

    /// <summary>
    /// Updates the psi/rpm of the pump
    /// </summary>
    private void UpdateDischarge()
    {
        switch (currentMode)
        {
            //Calculate new current pressure, set discharges, update audio
            case PanelMode.Pressure:
                if (targetPSI != currentPressure || currentIntake != prevIntake)
                {
                    calculatePSI();

                    pumpDischarge.SetInput(currentPressure / (float)maxPressure);
                    float _total = 0;
                    foreach (Discharge d in discharges)
                    {
                        d.SetMasterDischarge(currentPressure);
                        _total += d.GetDischarge();
                    }
                    totalPSI = _total;
                    currentRPM = ((currentPressure - currentIntake) * psiToRPM * (7f / 8f)) + (totalPSI * psiToRPM * (2f / 8f));
                    if (currentRPM < 0)
                    {
                        currentRPM = 0;
                    }
                    rpmText.text = currentRPM.ToString("0000");
                    engine.setRPM(currentRPM);

                    OnPressureChange.Invoke();
                }
                targetRPM = currentRPM;
                break;

            //Calculate new current rpm, set discharges, update audio
            case PanelMode.RPM:
                if (targetRPM != currentRPM || previousAngle != totalAngle || currentIntake != prevIntake || (!pumpEngaged && currentMode != PanelMode.Off))
                {
                    previousAngle = totalAngle;
                    calculateRPM();

                    currentPressure = ((8f * (currentRPM - (.25f * totalPSI * psiToRPM))) / (7f * psiToRPM)) + currentIntake;
                    if (currentPressure < 0)
                        currentPressure = 0;

                    foreach (Discharge _discharge in discharges)
                    {
                        _discharge.SetMasterDischarge(currentPressure);
                    }
                    psiText.text = currentPressure.ToString("0000");
                    engine.setRPM(currentRPM);

                    OnPressureChange.Invoke();
                }
                targetPSI = currentPressure;
                break;

            //Calculate new current pressure, set discharges, update audio
            case PanelMode.Preset:
                if (targetPSI != currentPressure || currentIntake != prevIntake)
                {
                    calculatePSI();

                    pumpDischarge.SetInput((float)currentPressure / (float)maxPressure);
                    float _total = 0;
                    foreach (Discharge d in discharges)
                    {
                        d.SetMasterDischarge(currentPressure);
                        _total += d.GetDischarge();
                    }
                    totalPSI = _total;
                    currentRPM = ((currentPressure - currentIntake) * psiToRPM * (7f / 8f)) + (totalPSI * psiToRPM * (2f / 8f));
                    if (currentRPM < 0)
                    {
                        currentRPM = 0;
                    }
                    rpmText.text = currentRPM.ToString("0000");
                    engine.setRPM(currentRPM);

                    OnPressureChange.Invoke();
                }
                else
                {
                    currentMode = previousMode;
                }
                targetRPM = currentRPM;
                break;

            //Calculate new current pressure, set discharges, update audio
            //Need to change so that it sets the target RPM to 0
            case PanelMode.Idle:
                if (targetPSI != currentPressure || currentIntake != prevIntake)
                {
                    calculatePSI();

                    pumpDischarge.SetInput((float)currentPressure / (float)maxPressure);
                    float _total = 0;
                    foreach (Discharge d in discharges)
                    {
                        d.SetMasterDischarge(currentPressure);
                        _total += d.GetDischarge();
                    }
                    totalPSI = _total;
                    currentRPM = ((currentPressure - currentIntake) * psiToRPM * (7f / 8f)) + (totalPSI * psiToRPM * (2f / 8f));
                    if (currentRPM < 0)
                    {
                        currentRPM = 0;
                    }
                    rpmText.text = currentRPM.ToString("0000");
                    engine.setRPM(currentRPM);

                    OnPressureChange.Invoke();
                }
                else
                {
                    currentMode = previousMode;
                }
                targetRPM = currentRPM;
                break;

            default:
                break;
        }
        //Update intake and set gauge
        prevIntake = currentIntake;
        rpmGauge.SetInput(currentRPM / 3000f);
        errorText.CheckDischarge(targetPSI);
    }

    /// <summary>
    /// Updates the current pressure towards the target Pressure
    /// </summary>
    private void calculatePSI()
    {
        if(currentPressure < targetPSI)
        {
            currentPressure += deltaPSI * Time.deltaTime;
            if (currentPressure > targetPSI)
                currentPressure = targetPSI;
        }
        else if(currentPressure > targetPSI)
        {
            currentPressure -= deltaPSI * Time.deltaTime;
            if (currentPressure < targetPSI)
                currentPressure = targetPSI;
        }

        if(currentPressure == targetPSI && !pumpEngaged)
        {
            currentMode = PanelMode.Off;
            engine.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the current rpm towards the target rpm
    /// </summary>
    private void calculateRPM()
    {
        if(currentRPM < targetRPM)
        {
            currentRPM += deltaRPM * Time.deltaTime;
            if(currentRPM > targetRPM)
            {
                currentRPM = targetRPM;
            }
        }
        else if(currentRPM > targetRPM)
        {
            currentRPM -= deltaRPM * Time.deltaTime;
            if(currentRPM < targetRPM)
            {
                currentRPM = targetRPM;
            }
        }
        if (currentRPM == targetRPM && !pumpEngaged)
        {
            currentMode = PanelMode.Off;
            engine.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Increases the TargetPSI by passed in delta
    /// </summary>
    /// <param name="_delta">Amount to change the target psi</param>
    private void IncreasePressure(int _delta)
    {
        if (!pumpEngaged || currentMode != PanelMode.Pressure)
            return;

        targetPSI += _delta;

        if (targetPSI < 0)
            targetPSI = 0;

        if (targetPSI > maxPressure)
            targetPSI = maxPressure;

        psiText.text = targetPSI.ToString("0000");
        errorText.CheckDischarge(targetPSI);
    }

    /// <summary>
    /// Increases the target rpm by the passed in delta
    /// </summary>
    /// <param name="_delta">Amount to change the rpm by</param>
    private void IncreaseRPM(int _delta)
    {
        if (!pumpEngaged || currentMode != PanelMode.RPM)
            return;

        targetRPM += _delta;

        if (targetRPM < 0)
            targetRPM = 0;

        if (targetRPM > maxRPM)
            targetRPM = maxRPM;

        rpmText.text = targetRPM.ToString("0000");
    }
    #endregion
}