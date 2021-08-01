using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainTank : MonoBehaviour
{
    [SerializeField] private TankStatus tankStatus;
    [SerializeField] private CheckGPM gpm;
    [SerializeField] private ErrorChecks errorChecks;
    private float tankVolume;
    private float currentGPS;
    private bool refillStatus;

    private const float REFILLSPEED = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        tankVolume = 500;
        currentGPS = 0;
        refillStatus = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGPS != (gpm.GetGPM() / 60))
        {
            currentGPS = gpm.GetGPM() / 60;
        }
        if (currentGPS > 0 && !tankStatus.GetExternalStatus())
        {
            if (tankVolume > 0)
            {
                tankVolume -= currentGPS * Time.deltaTime;
            }
            else
            {
                tankVolume = 0;
            }
            errorChecks.CheckTankVolume(tankVolume);
        }
        else if (currentGPS > 0 && tankStatus.GetExternalStatus())
        {
            if (tankVolume > 0)
            {
                tankVolume -= currentGPS * 0.25f * Time.deltaTime;
            }
            else
            {
                tankVolume = 0;
            }
            errorChecks.CheckTankVolume(tankVolume);
        }
        if (refillStatus == true)
        {
            if (tankVolume >= 500)
            {
                tankVolume = 500;
                refillStatus = false;
            }
            tankVolume += REFILLSPEED * Time.deltaTime;
            errorChecks.CheckTankVolume(tankVolume);
        }
    }

    public float GetTankVolume()
    {
        return tankVolume;
    }

    public void Refill()
    {
        refillStatus = true;
    }
}
