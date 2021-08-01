using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gauge))]
public class MirrorGauge : MonoBehaviour
{
    [Tooltip("Gauge to mirror")]
    [SerializeField] private Gauge mirroredGauge;

    private Gauge gauge;
    private float prevValue;

    private void Awake()
    {
        gauge = GetComponent<Gauge>();
    }

    private void Update()
    {
        if(mirroredGauge.GetOutputValue() != prevValue)
        {
            prevValue = mirroredGauge.GetOutputValue();
            gauge.SetInput(prevValue / gauge.GetMaxValue());
        }
    }
}
