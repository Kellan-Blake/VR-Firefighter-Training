using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncreaseCounter : MonoBehaviour
{

    public TextMeshPro rpmNumber;
    public TextMeshPro dischargeNumber;
    public TextMeshPro foamNumber;

    private float textNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void IncrementRPM()
    {
        float.TryParse(rpmNumber.text, out textNumber);
        Increment(rpmNumber, 10, 9990, "0000");
    }

    public void DecrementRPM()
    {
        float.TryParse(rpmNumber.text, out textNumber);
        Decrement(rpmNumber, 10, 9, "0000");
    }

    public void IncrementDischarge()
    {
        float.TryParse(dischargeNumber.text, out textNumber);
        Increment(dischargeNumber, 1, 600, "000");
    }

    public void DecrementDischarge()
    {
        float.TryParse(dischargeNumber.text, out textNumber);
        Decrement(dischargeNumber, 1, 0, "000");
    }

    public void IncrementFoam()
    {
        float.TryParse(foamNumber.text, out textNumber);
        Increment(foamNumber, .5f, 6, "0.0");
    }

    public void DecrementFoam()
    {
        float.TryParse(foamNumber.text, out textNumber);
        Decrement(foamNumber, .5f, 0, "0.0");
    }

    private void Increment(TextMeshPro number, float incrementBy, float max, string numDigits)
    {
        if (textNumber >= max)
        {
            return;
        }
        else
        {
            textNumber += incrementBy;
            number.text = textNumber.ToString(numDigits);
        }
    }

    private void Decrement(TextMeshPro number, float decrementBy, float min, string numDigits)
    {
        if (textNumber <= min)
        {
            return;
        }
        else
        {
            textNumber -= decrementBy;
            number.text = textNumber.ToString(numDigits);
        }
    }
}
