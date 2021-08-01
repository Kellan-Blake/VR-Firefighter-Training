
using UnityEngine;

public abstract class Discharge : MonoBehaviour
{
    [SerializeField] protected MasterDischarge master;
    [SerializeField] protected Gauge gauge;
    [SerializeField] private int index = -1;
    protected float PSI = 0;
    protected float angleValue = 0;
    protected float previousAngle = 0;
    protected float masterDischargePSI = 0;

    protected virtual void Awake()
    {
        master.AddDischarge(this);
        masterDischargePSI = master.GetPressure();
        UpdateGauge();
    }

    public virtual float GetPSI()
    {
        return PSI;
    }

    public virtual int GetIndex()
    {
        return index;
    }

    public virtual void SetMasterDischarge(float _PSI)
    {
        masterDischargePSI = _PSI;
        PSI = angleValue * masterDischargePSI;
    }

    public virtual float GetDischarge()
    {
        return PSI;
    }

    public abstract void UpdateAngle();

    protected virtual void UpdateGauge()
    {
        float _newPSI = angleValue * masterDischargePSI;
        float _deltaPSI = _newPSI - PSI;
        float _deltaAngle = angleValue - previousAngle;
        previousAngle = angleValue;
        PSI = _newPSI;
        gauge.SetInput(PSI / gauge.GetMaxValue());
        master.IncreaseTotalPSI(_deltaPSI);
        master.IncreaseTotalAngle(_deltaAngle);
    }
}
