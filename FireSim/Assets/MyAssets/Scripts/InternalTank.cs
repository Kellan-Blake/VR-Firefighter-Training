using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalTank : MonoBehaviour
{
    [SerializeField] private Lever tankPump;
    [SerializeField] private GetIntake intake;

    [SerializeField] private float maxPSI = 100;

    private float AngleValue;
    private float PrevAngleValue;

    private void Start()
    {
        AngleValue = tankPump.GetAngleValue();
        intake.SetTankIntake(AngleValue * maxPSI);
        PrevAngleValue = AngleValue;
    }

    private void Update()
    {
        AngleValue = tankPump.GetAngleValue();
        if(AngleValue != PrevAngleValue)
        {
            intake.SetTankIntake(AngleValue * maxPSI);
            PrevAngleValue = AngleValue;
        }
    }
}
