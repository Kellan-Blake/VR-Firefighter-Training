using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Director : MonoBehaviour
{

    public MasterDischarge masterDischarge;
    public GetIntake intake;
    public float correctDischarge;
    [Tooltip("The index of the discharge is the index of the correct PSI.\nThere should be the same amount of floats as there are discharges.")]
    public List<float> CorrectPSI = new List<float>();
    public DrainTank tank;
    public Bleeder officerBleeder;
    public Bleeder driverBleeder;
    public MasterIntake officerValve;
    public MasterIntake driverValve;
    public Canvas buttonCanvas;

    protected abstract void CheckResults();

    protected virtual void setTargetPSI()
    {
        Settings _settings = FindObjectOfType<Settings>();

        for (int i = 0; i < 9; i++)
        {
            //Current Hose settings
            float[] _dischargeSettings = _settings.GetExternalHose(i);

            //5 PSI per story
            float _elevationOffset = 5 * _dischargeSettings[2];

            //Correct nozzle pressure
            //TODO: Implement preconnect nozzle pressure
            float _correctPressure = SettingsManager.getNozzlePressure((SettingsManager.NozzleType)_dischargeSettings[3], (SettingsManager.HoseSize)_dischargeSettings[4]);

            //TODO: Calculate based off of hose size, distance, and GPM
            float _distanceOffset = 0;

            CorrectPSI[i] = _correctPressure + _distanceOffset + _elevationOffset;
        }
    }

    protected virtual void Start()
    {
        setTargetPSI();
    }
}
