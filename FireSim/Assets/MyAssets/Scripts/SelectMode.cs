using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectMode : MonoBehaviour
{

    private string selectedMode;

    public TextMeshPro pressureText;
    public TextMeshPro rpmText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setToPressure()
    {
        selectedMode = "Pressure";
        pressureText.color = Color.green;
        rpmText.color = Color.white;
    }

    public void setToRPM()
    {
        selectedMode = "RPM";
        rpmText.color = Color.green;
        pressureText.color = Color.white;
    }
}
