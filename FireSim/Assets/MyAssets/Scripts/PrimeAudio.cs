using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.Newtonsoft.Json.Bson;

/// <summary>
/// This class handles the audio handling of the engine prime sound
/// </summary>

public class PrimeAudio : MonoBehaviour
{
    #region Instance Variables

    [SerializeField] private AudioClip PrimeStart;
    [SerializeField] private AudioClip PrimeLoop;
    [SerializeField] private AudioClip PrimeLoopEnd;

    private AudioSource m_Start;
    private AudioSource m_Loop;
    private AudioSource m_End;

    private bool isTabPulled = false;
    private bool wasTabPulled = false;

    private bool loopEnd = false;

    private bool stopAudio = false;

    private bool reset = false;

    #endregion

    #region Unity Methods

    private void Start()
    {
        setupAudioSources();
    }

    private void Update()
    {
        CheckPrime();

        SetAudio();

        ResetAudio();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Initializes all the audio clips
    /// </summary>
    private void setupAudioSources()
    {
        m_Start = gameObject.AddComponent<AudioSource>();
        m_Start.clip = PrimeStart;

        m_Loop = gameObject.AddComponent<AudioSource>();
        m_Loop.clip = PrimeLoop;
        m_Loop.loop = true;

        m_End = gameObject.AddComponent<AudioSource>();
        m_End.clip = PrimeLoopEnd;
    }

    /// <summary>
    /// Checks the state of the primer
    /// </summary>
    private void CheckPrime()
    {
        if (isTabPulled != wasTabPulled)
        {
            if (isTabPulled)
            {
                StartAudio();
            }
            else
            {
                StopAudio();
            }

            wasTabPulled = isTabPulled;
        }
    }

    /// <summary>
    /// Sets the next audio clip to be played
    /// </summary>
    private void SetAudio()
    {
        if(stopAudio)
        {
            return;
        }

        if (m_Start.isPlaying)
        {
            float timeleft = m_Start.time;
            float maxTime = m_Start.clip.length;
            if (maxTime - timeleft <= .05f)
            {
                m_Loop.Play();
            }
        }

        if (m_Loop.isPlaying && !loopEnd && !m_Loop.loop)
        {
            float timeleft = m_Loop.time;
            float maxTime = m_Loop.clip.length;
            if (maxTime - timeleft <= .25)
            {
                m_End.Play();
                loopEnd = true;
            }
        }
    }

    /// <summary>
    /// Starts playing the prime audio
    /// </summary>
    private void StartAudio()
    {
        m_Start.Play();
    }

    /// <summary>
    /// Stops the priming audio
    /// </summary>
    private void StopAudio()
    {
        reset = true;
        //If still starting just stop
        if(m_Start.isPlaying)
        {
            m_Start.Stop();
        }

        //If in the looping audio, stop loop and play end audio
        else if(m_Loop.isPlaying)
        {
            float _timeLeft = m_Loop.clip.length - m_Loop.time;

            if(_timeLeft > .5)
            {
                StartCoroutine(StopLoop());
                stopAudio = true;
            }
            else
            {
                m_Loop.loop = false;
            }
        }
    }

    /// <summary>
    /// Resets instance variables to default values
    /// so that the audio can be played multiple times.
    /// </summary>
    private void ResetAudio()
    {
        if(m_Start.isPlaying || m_Loop.isPlaying || m_End.isPlaying || !reset)
        {
            return;
        }

        reset = false;
        stopAudio = false;
        loopEnd = false;
        m_Loop.loop = true;
    }

    /// <summary>
    /// Stops the looping audio and starts the end audio
    /// </summary>
    private IEnumerator StopLoop()
    {
        m_End.Play();

        yield return new WaitForSeconds(.25f);

        m_Loop.Stop();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts the prime sequence
    /// </summary>
    public void StartPrime()
    {
        isTabPulled = true;
    }

    /// <summary>
    /// Ends the prime sequence
    /// </summary>
    public void EndPrime()
    {
        isTabPulled = false;
    }

    #endregion
}