/// <summary>
/// This class manages a connector that acts as an intake connector to the pump panel
/// It will update the psi of the intake depending on the psi of the connected connector
/// </summary>

using UnityEngine;

public class ConnectorManager : MonoBehaviour
{
    [Tooltip("The connector that acts as the intake")]
    [SerializeField] private Connector connector;

    [Tooltip("Master Intake on the same side as the connector")]
    [SerializeField] private MasterIntake intake;

    //Current PSI
    private float PSI = 0;

    //Set the starting intake
    private void Start()
    {
        PSI += connector.PSI;
        intake.SetIntake(PSI);
    }

    //Add the delta PSI to the intake
    public void UpdateIntake()
    {
        PSI = connector.PSI;
        intake.SetIntake(PSI);
    }
}