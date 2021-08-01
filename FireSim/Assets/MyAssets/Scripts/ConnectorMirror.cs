using UnityEngine;

[RequireComponent(typeof(ConnectorMale))]

public class ConnectorMirror : MonoBehaviour
{
    [Tooltip("This is the connector that will be copied")]
    [SerializeField] private ConnectorFemale mirroredConnector;

    [Tooltip("Distance between this connector and the mirrored connector in feet")]
    [SerializeField] private float distance = 50;

    [Tooltip("Amount of psi lost to friction per foot")]
    [SerializeField] private float frictionLoss = 0.75f;

    [Tooltip("Amount of PSI lost to elevation per meter")]
    [SerializeField] private float elevationLoss = 1f;

    private ConnectorMale mirrorConnector;

    private void Start()
    {
        mirrorConnector = GetComponent<ConnectorMale>();
    }

    // Update is called once per frame
    void Update()
    {
        float elevation = this.transform.position.y - mirrorConnector.transform.position.y;
        mirrorConnector.PSI = mirroredConnector.PSI - (frictionLoss * distance) - (elevation * elevationLoss);
        mirrorConnector.GPM = mirroredConnector.GPM;
        if(mirrorConnector.connectedFemale != null)
        {
            mirrorConnector.connectedFemale.PSI = mirrorConnector.PSI;
            mirrorConnector.connectedFemale.GPM = mirrorConnector.GPM;
        }
    }
}
