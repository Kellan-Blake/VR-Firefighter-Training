using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the settings for all the scenarios
/// </summary>

public class Settings : MonoBehaviour
{
    #region Instance Variables

    //Settings of the hoses for the internal/tank, external, and relay scenarios
    private static List<float[]> tank = new List<float[]>(9);
    private static List<float[]> external = new List<float[]>(9);
    private static List<float[]> relay = new List<float[]>(9);

    //Instance is a singleton of the Settings Object
    public static Settings Instance;

    private ScenarioInfo scenarioInfo;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        //Singleton pattern for Settings
        if (Instance == null)
        {
            Instance = this;
            init();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///Sets the scenario info
    /// </summary>
    /// <param name="_scenario">Scenario Info Object</param>
    public void SetInfo(ScenarioInfo _scenario)
    {
        scenarioInfo = _scenario;
    }

    /// <summary>
    /// Sets the discharge's settings for the internal scenario
    /// </summary>
    /// <param name="_index">Index of the discharge to be set</param>
    /// <param name="_distance">Distance of the hose in feet</param>
    /// <param name="_elevation">Elevation above pump in stories</param>
    /// <param name="_nozzle">Nozzle type of the discharge line</param>
    /// <param name="_size">Size of the hose</param>
    public void SetTank(int _index, float _distance, float _elevation, SettingsManager.NozzleType _nozzle, SettingsManager.HoseSize _size)
    {
        if(_index >= 9)
        {
            return;
        }
        float[]  _hose = { _index, _distance, _elevation, (int)_nozzle, (int) _size};
        tank[_index] = _hose;
    }

    /// <summary>
    /// Sets the discharge's settings for the external scenario
    /// </summary>
    /// <param name="_index">Index of the discharge to be set</param>
    /// <param name="_distance">Distance of the hose in feet</param>
    /// <param name="_elevation">Elevation above pump in stories</param>
    /// <param name="_discharge">Pressure to set the discharge to</param>
    public void SetExternal(int _index, float _distance, float _elevation, SettingsManager.NozzleType _nozzle, SettingsManager.HoseSize _size)
    {
        if (_index >= 9)
        {
            return;
        }
        float[] _hose = { _index, _distance, _elevation, (int)_nozzle, (int)_size };
        external[_index] = _hose;
    }

    /// <summary>
    /// Sets the discharge's settings for the relay scenario
    /// </summary>
    /// <param name="_index">Index of the discharge to be set</param>
    /// <param name="_distance">Distance of the hose in feet</param>
    /// <param name="_elevation">Elevation above pump in stories</param>
    /// <param name="_discharge">Pressure to set the discharge to</param>
    public void SetRelay(int _index, float _distance, float _elevation, SettingsManager.NozzleType _nozzle, SettingsManager.HoseSize _size)
    {
        if (_index >= 9)
        {
            return;
        }
        float[] _hose = { _index, _distance, _elevation, (int)_nozzle, (int)_size };
        relay[_index] = _hose;
    }

    /// <summary>
    /// Returns the given index's discharge settings for the internal scenario
    /// </summary>
    /// <param name="_index">Index of discharge</param>
    /// <returns>Index, distance, elevaion, pressure</returns>
    public float[] GetInternalHose(int _index)
    {
        return tank[_index];
    }

    /// <summary>
    /// Returns the given index's discharge settings for the external scenario
    /// </summary>
    /// <param name="_index">Index of discharge</param>
    /// <returns>Index, distance, elevaion, pressure</returns>
    public float[] GetExternalHose(int _index)
    {
        return external[_index];
    }

    /// <summary>
    /// Returns the given index's discharge settings for the relay scenario
    /// </summary>
    /// <param name="_index">Index of discharge</param>
    /// <returns>Index, distance, elevaion, pressure</returns>
    public float[] GetRelayHose(int _index)
    {
        return relay[_index];
    }

    /// <summary>
    /// Loads the internal scene settings into scenario info
    /// </summary>
    public void LoadInternal()
    {
        foreach (float[]  _hose in tank)
        {
            //As long as the nozzle type is not zero
            if(_hose[3] > 0)
            {
                scenarioInfo.SetInfo((int)_hose[0], _hose[1], _hose[2], (SettingsManager.NozzleType)_hose[3], (SettingsManager.HoseSize) _hose[4]);
            }
        }
    }

    /// <summary>
    /// Loads the external scene settings into scenario info
    /// </summary>
    public void LoadExternal()
    {
        foreach (float[] _hose in external)
        {
            //As long as the nozzle type is not zero
            if (_hose[3] > 0)
            {
                scenarioInfo.SetInfo((int)_hose[0], _hose[1], _hose[2], (SettingsManager.NozzleType)_hose[3], (SettingsManager.HoseSize)_hose[4]);
            }
        }
    }

    /// <summary>
    /// Loads the relay scene settings into scenario info
    /// </summary>
    public void LoadRelay()
    {
        foreach (float[] _hose in relay)
        {
            //As long as the nozzle type is not zero
            if (_hose[3] > 0)
            {
                scenarioInfo.SetInfo((int)_hose[0], _hose[1], _hose[2], (SettingsManager.NozzleType)_hose[3], (SettingsManager.HoseSize)_hose[4]);
            }
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Initializes the hose line arrays/list
    /// </summary>
    private void init()
    {
        for(int i = 0; i < 9; i++)
        {
            float[] _hose = { i, 0, 0, 0, 0};
            tank.Add(_hose);
            external.Add(_hose);
            relay.Add(_hose);
        }
        //Default external scene settings
        float[] _defaultScenes = { 3, 300, 0, 1, 0};
        external[3] = _defaultScenes;
        _defaultScenes = new float[] { 5, 200, 0, 1, 0};
        external[5] = _defaultScenes;

        //Default tank only scene settings
        _defaultScenes = new float[] { 3, 150, 0, 1, 0};
        tank[3] = _defaultScenes;

        //Default relay scene settings
        _defaultScenes = new float[] { 5, 500, 0, 1, 0};
        relay[5] = _defaultScenes;
    }

    #endregion
}
