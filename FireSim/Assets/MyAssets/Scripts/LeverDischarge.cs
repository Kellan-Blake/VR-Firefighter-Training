using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverDischarge : Discharge
{
    [SerializeField] private Lever lever;

    protected override void Awake()
    {
        angleValue = lever.GetAngleValue();
        base.Awake();
    }

    public override void UpdateAngle()
    {
        angleValue = lever.GetAngleValue();
        UpdateGauge();
    }
}
