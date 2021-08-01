using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MasterDischarge : MonoBehaviour
{
    public UnityEvent OnPressureChange;

    [Range(1, 600)]
    [SerializeField] private int maxPressure;
    [SerializeField] private int psiToRPM = 2;
    [SerializeField] private PanelMode mode;
    [SerializeField] private PanelSet set;
    [SerializeField] private Gauge pumpDischarge;
    [SerializeField] private Gauge pumpIntake;
    [SerializeField] private GameObject pressureLight;
    [SerializeField] private GameObject rpmLight;
    [SerializeField] private GameObject pumpEngagedLight;
    [SerializeField] private TextMeshPro psiText;
    [SerializeField] private TextMeshPro rpmText;
    [SerializeField] private List<Discharge> discharges = new List<Discharge>();
    [SerializeField] private float correctPressure;
    [SerializeField] private TextMeshPro errorText;
    [SerializeField] private EngineAudio engine;
    [SerializeField] private Switch onSwitch;

    private float currentRPM = 0;
    private float currentPressure = 0;
    private float idleRPM = 0;
    private float idlePressure = 0;
    private float previousPressue = 0;
    private float previousRPM = 0;
    private float totalPSI = 0;

    private bool presetPressed = false;
    private bool pumpEngaged = false;

    private void Start()
    {
        engine.enabled = false;
    }

    private void Update()
    {
        if (previousPressue != currentPressure && pumpEngaged)
        {
            previousPressue = currentPressure;
            OnPressureChange.Invoke();
            pumpDischarge.SetInput((float)currentPressure / (float)maxPressure);
            psiText.text = currentPressure.ToString("0000");
            float _total = 0;
            foreach (Discharge d in discharges)
            {
                d.SetMasterDischarge(currentPressure);
                _total += d.GetDischarge();
            }
            totalPSI = _total;
            currentRPM = totalPSI * psiToRPM;
            rpmText.text = currentRPM.ToString("0000");
            engine.setRPM(currentRPM);
        }
    }

    public void AddDischarge(Discharge _discharge)
    {
        discharges.Add(_discharge);
    }

    public void TurnOn()
    {
        if (pumpEngaged)
        {
            return;
        }
        mode = PanelMode.Pressure;

        engine.enabled = true;
        pumpEngaged = true;

        engine.setMax(3000);
        pumpDischarge.SetInput(currentPressure / maxPressure);
        errorText.text = "";
        previousPressue = currentPressure;
        OnPressureChange.Invoke();
        pumpDischarge.SetInput((float)currentPressure / (float)maxPressure);
        psiText.text = currentPressure.ToString("0000");
        float _total = 0;
        foreach (Discharge d in discharges)
        {
            d.SetMasterDischarge(currentPressure);
            _total += d.GetDischarge();
        }
        totalPSI = _total;
        currentRPM = totalPSI * psiToRPM;
        rpmText.text = currentRPM.ToString("0000");
        engine.setRPM(currentRPM);

    }

    public enum PanelMode
    {
        Pressure,
        RPM
    }

    public enum PanelSet
    {
        Idle,
        Preset
    }

    public void IncreasePressure(int i)
    {
        currentPressure += i;
        if (currentPressure < 0)
            currentPressure = 0;
        if (currentPressure > maxPressure)
            currentPressure = maxPressure;
        if (currentPressure >= (correctPressure + 100))
        {
            errorText.text = "WARNING: Pressure too high for current scenario";
        }
        else if (currentPressure < (correctPressure + 100) && errorText.text == "WARNING: Pressure too high for current scenario")
        {
            errorText.text = "";
        }
    }

    public void Idle()
    {
        if (set == PanelSet.Idle)
            return;
        set = PanelSet.Idle;
        idlePressure = currentPressure;
        idleRPM = currentRPM;
        currentPressure = 20;
        currentRPM = 404;
    }

    public void Preset()
    {
        if (set == PanelSet.Preset)
            return;
        set = PanelSet.Preset;
        if (!presetPressed)
        {
            presetPressed = true;
            currentPressure = 175;
            currentRPM = idleRPM;
        }
        else
        {
            currentPressure = idlePressure;
            currentRPM = idleRPM;
        }
    }

    public float GetPressure()
    {
        return currentPressure;
    }

    public void RPMButton()
    {
        mode = PanelMode.RPM;
    }

    public void PressureButton()
    {
        mode = PanelMode.Pressure;
    }

    public PanelSet GetSetting()
    {
        return set;
    }

    public PanelMode GetMode()
    {
        return mode;
    }

    public void IncreaseTotalPSI(float _delta)
    {
        totalPSI += _delta;
        currentRPM = totalPSI * psiToRPM;
        rpmText.text = currentRPM.ToString("0000");
        engine.setRPM(currentRPM);
    }
}
