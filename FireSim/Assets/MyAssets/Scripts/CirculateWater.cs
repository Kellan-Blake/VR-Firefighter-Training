using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirculateWater : MonoBehaviour
{

    [SerializeField] private TankStatus tankStatus;
    [SerializeField] private DrainTank drainTank;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefillTank()
    {
        if (tankStatus.GetExternalStatus() && drainTank.GetTankVolume() < 500)
        {
            drainTank.Refill();
        }
    }
}
