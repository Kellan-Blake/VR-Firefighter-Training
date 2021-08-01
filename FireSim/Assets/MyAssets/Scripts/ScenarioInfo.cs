using UnityEngine;

public class ScenarioInfo : MonoBehaviour
{

    private string[] lineInfo;
    string newInfo = "";

    // Start is called before the first frame update
    void Start()
    {
        lineInfo = new string[9];
        for (int i = 0; i < 9; i++)
        {
            lineInfo[i] = "";
        }
    }

    public void SetInfo(int _lineIndex, float _newDistance, float _newElevation, SettingsManager.NozzleType _nozzle, SettingsManager.HoseSize _size)
    {
        SettingsManager.HoseSize _hose = (SettingsManager.HoseSize) _size;
        SettingsManager.NozzleType _nozzleType = (SettingsManager.NozzleType)_nozzle;

        if (_newElevation == 0)
        {
            newInfo = _newDistance + " feet of hose. Ground level. " + SettingsManager.GetNozzleName(_nozzle) + " nozzle. " + SettingsManager.GetHoseName(_size) + "\" Hose";
        }
        else
        {
            newInfo = _newDistance + " feet of hose." + _newElevation + " stories up. " + SettingsManager.GetNozzleName(_nozzle) + " nozzle. " + SettingsManager.GetHoseName(_size) + "\" Hose";
        }
        lineInfo[_lineIndex] += newInfo;
    }

    public string[] GetInfo()
    {
        return lineInfo;
    }
}
