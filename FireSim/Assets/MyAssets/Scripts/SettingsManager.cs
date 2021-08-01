using TMPro;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public Scenario scenario;
    public Discharge discharge;
    public NozzleType nozzle;
    public HoseSize hoseSize;

    public Settings settings;

    public TMP_Dropdown DischargeDrop;
    public TMP_Dropdown NozzleDrop;
    public TMP_Dropdown HoseSizeDrop;

    public TMP_InputField distance;
    public TMP_InputField elevation;

    private float _pressure;
    private float _distance;
    private float _elevation;

    #region Hose Size Enum

    /// <summary>
    /// Different Hose sizes for the engine
    /// </summary>
    public enum HoseSize
    {
        One3_4,
        Two1_2,
        Four
    }

    /// <summary>
    /// Returns a readable string for the hose size
    /// </summary>
    /// <param name="_hose"></param>
    /// <returns>String format of hose size</returns>
    public static string GetHoseName(HoseSize _hose)
    {
        switch (_hose)
        {
            case HoseSize.One3_4:
                return "1 3/4";

            case HoseSize.Two1_2:
                return "2 1/2";

            case HoseSize.Four:
                return "4";

            default:
                return "Hose is not a valid size";
        }
    }

    #endregion

    #region Nozzle Type Enum

    /// <summary>
    /// Different Nozzle Types for the fire department
    /// </summary>
    public enum NozzleType
    {
        Unused,
        MidMaticOne3_4,
        SmoothBore15_16,
        AutoMaticTwo1_2,
        SmoothBoreOne1_8,
        FogNozzle
    }

    /// <summary>
    /// Returns a readable string format of the nozzle type
    /// </summary>
    /// <param name="_nozzle">Nozzle to get name of</param>
    /// <returns>String format of the nozzle</returns>
    public static string GetNozzleName(NozzleType _nozzle)
    {
        switch (_nozzle)
        {
            case NozzleType.MidMaticOne3_4:
                return "1 3/4 - Mid-Matic";
            case NozzleType.SmoothBore15_16:
                return "15/16  - Smooth Bore";
            case NozzleType.AutoMaticTwo1_2:
                return "2 1/2 - Auto-Matic";
            case NozzleType.SmoothBoreOne1_8:
                return "1 1/8 - Smooth Bore";
            case NozzleType.FogNozzle:
                return "Fog Nozzle";
            default:
                return "Not in use";
        }
    }

    #endregion

    #region Discharge Enum

    /// <summary>
    /// Names of the discharges on the current engine
    /// </summary>
    public enum Discharge
    {
        NO4_OFFICER,
        NO2_OFFICER,
        OFFICER_SIDE_REAR,
        OFFICER_SIDE_CROSSLAY,
        DELUGE,
        DRIVER_SIDE_CROSSLAY,
        DRIVER_REAR,
        NO3_DRIVER,
        NO1_DRIVER
    }

    /// <summary>
    /// Returns a readable string format of the discharge
    /// </summary>
    /// <param name="_nozzle">discharge to get name of</param>
    /// <returns>String format of the discharge</returns>
    public static string GetDischargeName(Discharge _discharge)
    {
        switch(_discharge)
        {
            case Discharge.NO4_OFFICER:
                return "NO. 4 Officers Side Discharge";
            case Discharge.NO2_OFFICER:
                return "NO. 2 Officers Side Discharge";
            case Discharge.OFFICER_SIDE_REAR:
                return "Officer Side Rear Large Discharge";
            case Discharge.OFFICER_SIDE_CROSSLAY:
                return "Officer Side Crosslay Water-Foam";
            case Discharge.DELUGE:
                return "Deluge Discharge";
            case Discharge.DRIVER_SIDE_CROSSLAY:
                return "Driver Side Crosslay Water-Foam";
            case Discharge.DRIVER_REAR:
                return "Driver Rear Discharge Water-Foam";
            case Discharge.NO3_DRIVER:
                return "NO. 3 Driver Side Discharge";
            case Discharge.NO1_DRIVER:
                return "NO. 1 Driver Side Discharge";
            default:
                return "Unused Discharge";
        }
    }
    #endregion

    //Scenario Types
    public enum Scenario
    {
        Tank,
        External,
        Relay
    }

    //Sets the scenario to change
    public void SetScenario(int _index)
    {
        scenario = (Scenario)_index;
        loadSettings();
    }

    //Sets the discharge to change
    public void SetDischarge()
    {
        setHose();
        discharge = (Discharge)DischargeDrop.value;
        loadSettings();
    }

    public void SetNozzle()
    {
        nozzle = (NozzleType)NozzleDrop.value;
        setHose();
    }

    public void SetHoseSize()
    {
        hoseSize = (HoseSize)HoseSizeDrop.value;
        setHose();
    }

    //Sets the elevation of the active scenario's discharge
    public void SetElevation()
    {
        if (elevation.text != "")
        {
            _elevation = float.Parse(elevation.text);
        }
        else
        {
            _elevation = 0;
        }
        setHose();
    }

    //Sets the distance of the active scenario's discharge
    public void SetDistance()
    {
        if (distance.text != "")
        {
            _distance = float.Parse(distance.text);
        }
        else
        {
            _distance = 0;
        }
        setHose();
    }

    /// <summary>
    /// Returns the pressure rating of the nozzle
    /// </summary>
    /// <param name="_nozzle">SettingsManger.Nozzle Type</param>
    /// <param name="_size">SettingsManger.Hose Size</param>
    /// <returns>Pressure of nozzle</returns>
    public static float getNozzlePressure(NozzleType _nozzle, HoseSize _size)
    {
        float pressure = 0;
        switch (_nozzle)
        {
            case NozzleType.MidMaticOne3_4:
                pressure = 75;
                break;

            case NozzleType.SmoothBore15_16:
                pressure = 50;
                break;

            case NozzleType.AutoMaticTwo1_2:
                pressure = 100;
                break;

            case NozzleType.SmoothBoreOne1_8:
                pressure = 50;
                break;

            case NozzleType.FogNozzle:
                switch (_size)
                {
                    case HoseSize.One3_4:
                        pressure = 100;
                        break;

                    case HoseSize.Two1_2:
                        pressure = 100;
                        break;

                    default:
                        pressure = 0;
                        break;

                }
                break;

            default:
                pressure = 0;
                break;
        }

        return pressure;
    }

    /// <summary>
    /// Returns the pressure rating of the nozzle on a pre connect/crosslay
    /// </summary>
    /// <param name="_nozzle">SettingsManger.Nozzle Type</param>
    /// <param name="_size">SettingsManger.Hose Size</param>
    /// <returns>Pressure of nozzle</returns>
    public static float getPreConNozzlePressure(NozzleType _nozzle, HoseSize _size)
    {
        float pressure = 0;
        switch (_nozzle)
        {
            case NozzleType.MidMaticOne3_4:
                pressure = 160;
                break;

            case NozzleType.SmoothBore15_16:
                pressure = 165;
                break;

            case NozzleType.AutoMaticTwo1_2:
                pressure = 130;
                break;

            case NozzleType.SmoothBoreOne1_8:
                pressure = 80;
                break;

            case NozzleType.FogNozzle:
                switch (_size)
                {
                    case HoseSize.One3_4:
                        pressure = 100;
                        break;

                    case HoseSize.Two1_2:
                        pressure = 100;
                        break;

                    default:
                        pressure = 0;
                        break;

                }
                break;

            default:
                pressure = 0;
                break;
        }

        return pressure;
    }

    //Saves the settings of the active scenario's discharge in the settings
    private void setHose()
    {
        switch(scenario)
        {
            case Scenario.Tank:
                settings.SetTank((int)discharge, _distance, _elevation, nozzle, hoseSize);
                break;

            case Scenario.External:
                settings.SetExternal((int)discharge, _distance, _elevation, nozzle, hoseSize);
                break;

            case Scenario.Relay:
                settings.SetRelay((int)discharge, _distance, _elevation, nozzle, hoseSize);
                break;
        }
    }

    private void loadSettings()
    {
        float[] _hose;
        switch (scenario)
        {
            case Scenario.Tank:
                _hose = settings.GetInternalHose((int)discharge);
                break;

            case Scenario.External:
                _hose = settings.GetExternalHose((int)discharge);
                break;

            default:
                _hose = settings.GetRelayHose((int)discharge);
                break;
        }
        NozzleDrop.SetValueWithoutNotify((int)_hose[3]);
        nozzle = (NozzleType)_hose[3];
        HoseSizeDrop.SetValueWithoutNotify((int) _hose[4]);
        hoseSize = (HoseSize)_hose[4];
        elevation.text = _hose[2].ToString();
        distance.text = _hose[1].ToString();

        _elevation = _hose[2];
        _distance = _hose[1];

        if (_hose[2] <= 0)
        {
            elevation.text = "";
        }
        if (_hose[1] <= 0)
        {
            distance.text = "";
        }
    }
}
