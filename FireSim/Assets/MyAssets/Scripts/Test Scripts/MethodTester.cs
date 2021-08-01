using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MethodTester : MonoBehaviour
{

    public UnityEvent Bool1;
    public UnityEvent Bool2;

    public bool tester1 = false;

    public bool tester2 = false;

    private void Update()
    {
        if(tester1)
        {
            tester1 = false;
            Bool1.Invoke();
        }

        if (tester2)
        {
            tester2 = false;
            Bool2.Invoke();
        }
    }
}
