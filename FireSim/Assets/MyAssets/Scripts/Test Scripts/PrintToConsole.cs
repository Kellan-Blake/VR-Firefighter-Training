using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintToConsole : MonoBehaviour
{
    [SerializeField] private string message;

    public void ConsolePrint()
    {
        Debug.Log(message);
    }
}
