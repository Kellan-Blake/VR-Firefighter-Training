using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CheckTank : MonoBehaviour
{

    public Lever tankLever;
    public GameObject externalSource;

    private TankStatus tankStatus;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().enabled = false;
        tankStatus = tankLever.GetComponent<TankStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tankStatus.IsTankOpen())
        {
            GetComponent<Button>().enabled = true;
        }
        else
        {
            GetComponent<Button>().enabled = false;
        }
    }


}
