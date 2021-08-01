using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ValveIntake : MonoBehaviour
{

    [SerializeField] private UnityEvent ValveOpen;
    [SerializeField] private UnityEvent ValveClosed;
    [SerializeField] private UnityEvent ValveOff;

    [SerializeField] private TankStatus tankStatus;

    public GameObject hose;
    public Bleeder bleeder;
    public GetIntake masterIntake;

    private float countdown;
    private bool startOpen;
    private bool startClose;
    private float maxCountdown;
    private float intakeAmount;
    private bool intakeIncreased;
    private float prevCountdown;
    private bool hoseAttached;

    // Start is called before the first frame update
    void Start()
    {
        countdown = 0;
        maxCountdown = 5.0f;
        intakeAmount = 50;
        prevCountdown = countdown;
        ValveClosed.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCountdown();
    }

    public void StartOpening()
    {
        startOpen = true;
        startClose = false;
        bleeder.SetValveStatus(hoseAttached);
        ValveOpen.Invoke();
        tankStatus.SetExternal(hoseAttached);
    }

    public void StartClosing()
    {
        startClose = true;
        startOpen = false;
    }

    public void DoNothing()
    {
        startClose = false;
        startOpen = false;
    }

    private void UpIntake()
    {
        if (!intakeIncreased)
        {
            return;
        }
        masterIntake.IncreaseIntake(((countdown - prevCountdown) / maxCountdown) * intakeAmount);
    }

    private void DecreaseIntake()
    {
        if (!intakeIncreased)
        {
            return;
        }
        masterIntake.DecreaseIntake(((prevCountdown - countdown) / maxCountdown) * intakeAmount);
    }

    public void FirstIntakeIncrease()
    {
        masterIntake.IncreaseIntake((countdown / maxCountdown) * intakeAmount);
        intakeIncreased = true;
    }

    private void UpdateCountdown()
    {
        if (startOpen && countdown < maxCountdown)
        {
            prevCountdown = countdown;
            countdown += Time.deltaTime;
            if (hoseAttached)
            {
                UpIntake();
            }
        }
        else if (startClose && countdown > 0)
        {
            prevCountdown = countdown;
            countdown -= Time.deltaTime;
            if (hoseAttached)
            {
                DecreaseIntake();
            }
        }
        else if (startClose && countdown <= 0)
        {
            bleeder.SetValveStatus(false);
            ValveClosed.Invoke();
            intakeIncreased = false;
        }
    }

    public void HoseAttached()
    {
        hoseAttached = true;
    }

    public void HoseDetached()
    {
        prevCountdown = countdown;
        countdown = 0;
        bleeder.SetValveStatus(false);
        ValveOff.Invoke();
        intakeIncreased = false;
        masterIntake.DecreaseIntake(((prevCountdown) / maxCountdown) * intakeAmount);
    }
}
