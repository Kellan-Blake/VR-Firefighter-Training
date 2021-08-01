using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankDischarge : Discharge
{
    [SerializeField] private HandCrank crank;

    protected override void Awake()
    {
        angleValue = crank.GetAngleValue();
        base.Awake();
    }

    public override void UpdateAngle()
    {
        angleValue = crank.GetAngleValue();
        UpdateGauge();
    }
}