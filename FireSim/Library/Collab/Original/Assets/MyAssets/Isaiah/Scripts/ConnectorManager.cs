using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorManager : MonoBehaviour
{
    [SerializeField] private List<Connector> connectors = new List<Connector>();
    [SerializeField] private GetIntake intake;

    private float totalPSI = 0;
    private float prevPSI = 0;

    private void Start()
    {
        foreach(Connector c in connectors)
        {
            totalPSI += c.PSI;
        }
        intake.IncreaseIntake(totalPSI);
        prevPSI = totalPSI;
    }

    public void UpdateIntake()
    {
        /*totalPSI = 0;
        foreach (Connector c in connectors)
        {
            totalPSI += c.PSI;
        }
        float deltaPSI = totalPSI - prevPSI;
        intake.IncreaseIntake(deltaPSI);
        prevPSI = totalPSI;*/
    }
}