using UnityEngine;

public class EngineAudio : MonoBehaviour
{
    public AudioClip lowAccelClip;                                              // Audio clip for low acceleration
    public AudioClip lowDecelClip;                                              // Audio clip for low deceleration
    public AudioClip medAccelClip;                                             // Audio clip for high acceleration
    public AudioClip medDecelClip;                                             // Audio clip for high deceleration
    public AudioClip highAccelClip;                                             // Audio clip for high deceleration
    public AudioClip highDecelClip;                                             // Audio clip for high deceleration
    public float pitchMultiplier = 1f;                                          // Used for altering the pitch of audio clips
    public float lowPitchMin = 1f;                                              // The lowest possible pitch for the low sounds
    public float lowPitchMax = 3f;                                               // The highest possible pitch for the low sounds
    public float medPitchMultiplier = 0.25f;                                   // Used for altering the pitch of high sounds
    public float highPitchMultiplier = 0.1f;                                   // Used for altering the pitch of high sounds
    public float maxRolloffDistance = 500;                                      // The maximum distance where rollof starts to take place
    public float dopplerLevel = 1;                                              // The mount of doppler effect used in the audio
    public bool useDoppler = true;                                              // Toggle for using doppler

    private AudioSource m_LowAccel; // Source for the low acceleration sounds
    private AudioSource m_LowDecel; // Source for the low deceleration sounds
    private AudioSource m_MedDecel; // Source for the low deceleration sounds
    private AudioSource m_MedAccel; // Source for the low deceleration sounds
    private AudioSource m_HighAccel; // Source for the high acceleration sounds
    private AudioSource m_HighDecel; // Source for the high deceleration sounds

    private float currentRPM;
    private float prevRPM;
    private float accel;
    public float medRPM;
    private float maxRPM;

    private void Awake()
    {
        if(maxRPM <= 0)
        maxRPM = 1;
    }

    private void Start()
    {
        currentRPM = 0;
        prevRPM = 0;

        m_LowAccel = SetUpEngineAudioSource(lowAccelClip);
        m_LowDecel = SetUpEngineAudioSource(lowDecelClip);
        m_MedAccel = SetUpEngineAudioSource(medAccelClip);
        m_MedDecel = SetUpEngineAudioSource(medDecelClip);
        m_HighDecel = SetUpEngineAudioSource(highDecelClip);
        m_HighAccel = SetUpEngineAudioSource(highAccelClip);

        accel = (currentRPM - prevRPM) / Time.deltaTime;
        prevRPM = currentRPM;
        blend();
    }

    private void StopSound()
    {
        //Destroy all audio sources on this object:
        foreach (var source in GetComponents<AudioSource>())
        {
            Destroy(source);
        }
    }

    private void Update()
    {
        if (currentRPM == prevRPM)
            return;
        accel = (currentRPM - prevRPM) / Time.deltaTime;
        prevRPM = currentRPM;
        blend();
    }

    public void setRPM(float _rpm)
    {
        if (_rpm > maxRPM)
            _rpm = maxRPM;
        if (_rpm < 0)
            _rpm = 0;
        currentRPM = _rpm;
    }

    public void setMax(float _max)
    {
        if (_max < 1)
            _max = 1;
        maxRPM = _max;
    }

    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        // create the new audio source component on the game object and set up its properties
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.loop = true;

        // start the clip from a random point
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.maxDistance = maxRolloffDistance;
        source.dopplerLevel = 0;
        return source;
    }

    private void blend()
    {
        // The pitch is interpolated between the min and max values, according to the car's revs.
        float pitch = ULerp(lowPitchMin, lowPitchMax, currentRPM / maxRPM);

        // clamp to minimum pitch (note, not clamped to max for high revs while burning out)
        pitch = Mathf.Min(lowPitchMax, pitch);

        // adjust the pitches based on the multipliers
        m_LowAccel.pitch = pitch * pitchMultiplier;
        m_LowDecel.pitch = pitch * pitchMultiplier;
        m_MedAccel.pitch = pitch * medPitchMultiplier * pitchMultiplier;
        m_MedDecel.pitch = pitch * medPitchMultiplier * pitchMultiplier;
        m_HighAccel.pitch = pitch * highPitchMultiplier * pitchMultiplier;
        m_HighDecel.pitch = pitch * highPitchMultiplier * pitchMultiplier;

        // get values for fading the sounds based on the acceleration
        float accFade = Mathf.Abs(currentRPM/maxRPM);
        float decFade = 1 - accFade;

        // get the high fade value based on the cars revs
        float highFade = Mathf.InverseLerp(0f, 0.8f, currentRPM / maxRPM);
        float med = currentRPM / medRPM;
        float lowFade = 1 - highFade;
        if (med > 1)
        {
            med = 1 - highFade;
            lowFade = 0;
        }

        // adjust the values to be more realistic
        highFade = 1 - ((1 - highFade) * (1 - highFade));
        med = 1 - ((1 - med) * (1 - med));
        lowFade = 1 - ((1 - lowFade) * (1 - lowFade));
        accFade = 1 - ((1 - accFade) * (1 - accFade));
        decFade = 1 - ((1 - decFade) * (1 - decFade));

        // adjust the source volumes based on the fade values
        m_LowAccel.volume = lowFade * accFade;
        m_LowDecel.volume = lowFade * decFade;
        m_MedAccel.volume = med * accFade;
        m_MedDecel.volume = med * decFade;
        m_HighAccel.volume = highFade * accFade;
        m_HighDecel.volume = highFade * decFade;

        // adjust the doppler levels
        m_MedAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
        m_LowAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
        m_MedDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;
        m_LowDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;
        m_HighDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;
        m_HighAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
    }

    private static float ULerp(float from, float to, float value)
    {
        if (from < 0)
            from = 0;
        return (1.0f - value) * from + value * to;
    }
}
