using UnityEngine;

public class CrankToGauge : MonoBehaviour
{
    [SerializeField] private HandCrank c;
    [SerializeField] private Gauge gauge;
    [SerializeField] private float masterIntakePressure;
    private float maxGaugeValue;

    private void Start()
    {
        maxGaugeValue = gauge.GetMaxValue();
    }

    public void SetGauge()
    {
        float inputValue = (c.GetAngleValue() / (maxGaugeValue / masterIntakePressure));
        gauge.SetInput(inputValue);
    }

    public void SetMasterPressure(int i)
    {
        masterIntakePressure = i;
        SetGauge();
    }
}
