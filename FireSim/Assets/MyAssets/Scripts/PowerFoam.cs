using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerFoam : MonoBehaviour
{

    public TextMeshPro foamDisplay;

    // Start is called before the first frame update
    void Start()
    {
        foamDisplay.enabled = false;
    }


    public void SetPower()
    {
        if (foamDisplay.enabled)
        {
            foamDisplay.enabled = false;
        }
        else
        {
            foamDisplay.enabled = true;
            foamDisplay.text = "0.5";
        }
    }
}
