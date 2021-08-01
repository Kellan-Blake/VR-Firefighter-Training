using UnityEngine;

public class Slider : MonoBehaviour
{
    public EngineAudio EngineAudio;
    [Range(0, 8000)]
    public float RPM;

    [Range(0, 8000)]
    public float MaxRPM;

    private void Awake()
    {
        if (RPM > MaxRPM)
            RPM = MaxRPM;
        EngineAudio.setMax(MaxRPM);
        EngineAudio.setRPM(RPM);
    }

    private void Update()
    {
        EngineAudio.setRPM(RPM);
        EngineAudio.setMax(MaxRPM);
    }

}
