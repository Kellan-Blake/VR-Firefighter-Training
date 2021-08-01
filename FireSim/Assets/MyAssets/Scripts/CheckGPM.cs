using System.Collections.Generic;
using UnityEngine;

public class CheckGPM : MonoBehaviour
{

    #region Instance Variables
    public MasterDischarge masterDischarge;
    public GetIntake intake;
    public ErrorChecks errorChecks;

    private float dischargeGPM;
    private float currentTotalDischarge;
    private List<Discharge> discharges;

    private const float MAXGPM = 150;
    private const float PRESSUREFORMAX = 200;
    #endregion

    #region Unity Methods
    void Start()
    {
        discharges = masterDischarge.GetDischarges();
        currentTotalDischarge = masterDischarge.GetTotalDischarge();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTotalDischarge != masterDischarge.GetTotalDischarge())
        {
            currentTotalDischarge = masterDischarge.GetTotalDischarge();
            dischargeGPM = 0;
            foreach (Discharge d in discharges)
            {
                if (((d.GetDischarge() / PRESSUREFORMAX) * MAXGPM) > 150)
                {
                    dischargeGPM += 150;
                }
                else
                {
                    dischargeGPM += (d.GetDischarge() / PRESSUREFORMAX) * MAXGPM;
                }
            }
            errorChecks.CheckGPM(dischargeGPM);
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Returns the discharge GPM
    /// </summary>
    /// <returns></returns>
    public float GetGPM()
    {
        return dischargeGPM;
    }
    #endregion
}
