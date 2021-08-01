/// <summary>
/// This class manages the master intakes gates/valves. When the master intake is closed (angle = 0)
/// Then the intake will not increase. When the intake is fully opened (angle = 90) then the intake will
/// increase by the PSI. Values inbetween are interpolated between 0 and the PSI depending on the angle.
/// </summary>

using UnityEngine;

public class MasterIntake : MonoBehaviour
{
    //The lights the indicate if the valve is opened or not
    [SerializeField] private GameObject ClosedLight;
    [SerializeField] private GameObject SemiLight;
    [SerializeField] private GameObject OpenedLight;

    [SerializeField] private ErrorChecks errorChecks;

    [SerializeField] private Bleeder bleederValve;

    //Reference to change the intake of
    [SerializeField] private GetIntake intake;

    [SerializeField] private Intake side = Intake.officer;

    //Current max PSI
    private float PSI = 0;
    //prev max PSI
    private float prevPSI = 0;
    //Current angle of the gate 0 = closed
    private float angle = 0;
    //Set to 1 so that the lights are updated correctly on start
    private float prevAngle = 1;
    //Gate/Valve can open 90 degrees
    private float maxAngle = 90;
    //Valve/Gate should open in three seconds
    private float deltaAngle = 90f / 3f;

    //Start in a stale state
    private State state = State.stale;

    private void Start()
    {
        UpdateLight();
    }

    //Update the angle, intake, and then light each frame
    private void Update()
    {
        UpdateAngle();
        UpdateIntake();
        UpdateLight();
        UpdateValveStatus();
    }

    #region Public Methods

    //Starts opening the intake valve
    public void StartOpening()
    {
        state = State.opening;
    }

    //Starts to close the intake valve
    public void StartClosing()
    {
        state = State.closing;
    }

    //Stops the valve from opening/closing
    public void Stop()
    {
        state = State.stale;
    }

    //Set the max PSI
    public void SetIntake(float _intake)
    {
        PSI = _intake;
        if (_intake <= 0)
        {
            errorChecks.SetExternalStatus(false);
        }
        else
        {
            errorChecks.SetExternalStatus(true);
        }
    }

    public float GetPSI()
    {
        return PSI;
    }

    public float GetAngle()
    {
        return angle;
    }
    #endregion

    #region Private Methods

    //States that a master intake can be in
    private enum State
    {
        closing,
        opening,
        stale
    }

    private enum Intake
    {
        driver,
        officer
    }

    //Updates the current angle of the master intake
    private void UpdateAngle()
    {
        switch (state)
        {
            //If opening, increase the angle
            case State.opening:
                angle += deltaAngle * Time.deltaTime;
                break;

            //If closing, decrease the angle
            case State.closing:
                angle -= deltaAngle * Time.deltaTime;
                break;

            //If stale, do nothing
            default:
                break;
        }

        //If valve/gate finished closing, set the state to stale
        if (angle < 0)
        {
            angle = 0;
            state = State.stale;
        }

        //If the valve/gate finished opening, set the state to stale
        else if (angle > maxAngle)
        {
            angle = maxAngle;
            state = State.stale;
        }
    }

    private void UpdateIntake()
    {
        //Change on updated PSI
        if (PSI != prevPSI || angle != prevAngle)
        {
            //Previous angle is used in case the angle is updated and then will be resolved in the next if
            float _angleFrac = angle / maxAngle;
            float _newIntake = (PSI * _angleFrac);
            switch (side)
            {
                case Intake.officer:
                    intake.SetOfficerIntake(_newIntake);
                    break;
                case Intake.driver:
                    intake.SetDriverIntake(_newIntake);
                    break;
                default:
                    break;
            }
            prevPSI = PSI;
        }
    }

    private void UpdateLight()
    {
        //Change on updated angle
        if (angle != prevAngle)
        {
            //Closed
            if (angle <= 0)
            {
                ClosedLight.SetActive(true);
                SemiLight.SetActive(false);
                OpenedLight.SetActive(false);
            }
            //Middle
            else if (angle >= maxAngle)
            {
                ClosedLight.SetActive(false);
                SemiLight.SetActive(false);
                OpenedLight.SetActive(true);
            }
            //Open
            else
            {
                ClosedLight.SetActive(false);
                SemiLight.SetActive(true);
                OpenedLight.SetActive(false);
            }
        }
        prevAngle = angle;
    }

    private void UpdateValveStatus()
    {
        if (PSI > 0 && angle > 0)
        {
            bleederValve.SetValveStatus(true);
        }
        else
        {
            bleederValve.SetValveStatus(false);
        }
    }
    #endregion
}