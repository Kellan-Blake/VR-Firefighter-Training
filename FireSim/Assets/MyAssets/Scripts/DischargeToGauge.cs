using UnityEngine;

public class DischargeToGauge : MonoBehaviour
{
    [SerializeField] private Lever lever;
    [SerializeField] private Gauge gauge;
    [SerializeField] private float masterIntakePressure;
    private float maxGaugeValue;

    private void Start()
    {
        maxGaugeValue = gauge.GetMaxValue();
    }

    public void SetGauge()
    {
        float inputValue = 0;
        if (masterIntakePressure != 0)
        {
            inputValue = (lever.GetAngleValue() / (maxGaugeValue / masterIntakePressure));
        }
        gauge.SetInput(inputValue);
    }

    public void SetMasterPressure(int i)
    {
        masterIntakePressure = i;
        SetGauge();
    }
}
