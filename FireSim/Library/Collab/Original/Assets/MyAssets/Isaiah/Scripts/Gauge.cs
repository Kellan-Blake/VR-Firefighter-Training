using UnityEngine;

public class Gauge : MonoBehaviour
{
    [SerializeField] private int maxValue;
    [SerializeField] private int minValue;
    [SerializeField] private float maxRotationAngle = 270;
    [SerializeField] private float minRotationAngle = -20.78f;
    [SerializeField] private float zeroRotationAngle = 0;

    [SerializeField] private float inputValue = 0;
    [SerializeField] private float ouputValue = 0;

    private Quaternion startRotation;
    private float previousInputValue = 0;

    private float timer = 1.0f;


    void Start()
    {
        startRotation = transform.localRotation;
        previousInputValue = inputValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (previousInputValue != inputValue)
        {
            calculateValue();
            previousInputValue = inputValue;
        }
    }

    public void SetInput(float f)
    {
        inputValue = f;
    }

    public int GetMaxValue()
    {
        return maxValue;
    }

    private void calculateValue()
    {
        float angleToSet = inputValue * maxRotationAngle;
        float angleValue = inputValue * maxValue;
        if (inputValue >= 0)
        {
            if (angleToSet > maxRotationAngle)
            {
                angleToSet = maxRotationAngle;
                angleValue = maxValue;
            }
            Vector3 newRotation = startRotation.eulerAngles;
            newRotation.z += angleToSet;
            transform.localRotation = Quaternion.Euler(newRotation);
            ouputValue = angleValue;
        }
        else if(inputValue < 0)
        {
            if(angleToSet < minRotationAngle)
            {
                angleToSet = minRotationAngle;
                angleValue = minValue;
            }
            Vector3 newRotation = startRotation.eulerAngles;
            newRotation.z += angleToSet;
            transform.localRotation = Quaternion.Euler(newRotation);
            ouputValue = angleValue;
        }
    }

    public float GetOutputValue()
    {
        return ouputValue;
    }
}
