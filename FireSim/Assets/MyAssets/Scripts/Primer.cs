using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the prime function of the pump engine
/// </summary>

public class Primer : MonoBehaviour
{
    #region Instance Variables

    [Tooltip("How long to prime the pump")]
    [SerializeField] private float timer = 1.0f;

    public bool tabPulled = false;
    private bool isPrimed = false;

    [Tooltip("Audio source for the primer")]
    [SerializeField] private PrimeAudio PrimerAudio;

    [Tooltip("Tank of the pump")]
    [SerializeField] private TankStatus Tank;

    [Tooltip("The governer of the panel.")]
    [SerializeField] private MasterDischarge panel;

    #endregion

    #region Unity Methods
    
    private void Update()
    {
        if(!panel.PumpOn())
        {
            if(tabPulled)
            {
                PrimerAudio.EndPrime();
            }
            return;
        }

        if(!isPrimed && tabPulled)
        {
            if(timer <= 0)
            {
                isPrimed = true;
                Tank.setPrime(true);
            }
            
            timer -= Time.deltaTime;
        }
    }

#endregion

    #region Private Methods

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts the prime sequence
    /// </summary>
    public void PullTab()
    {
        if (!tabPulled)
        {
            tabPulled = true;
            PrimerAudio.StartPrime();
        }
    }

    /// <summary>
    /// Ends the prime sequence
    /// </summary>
    public void ReleaseTab()
    {
        if (tabPulled)
        {
            tabPulled = false;
            if (timer > 0)
            {
                timer = 1;
            }
            PrimerAudio.EndPrime();
        }
    }

    #endregion
}
