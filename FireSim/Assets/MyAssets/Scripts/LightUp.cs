using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUp : MonoBehaviour
{

    public GameObject button;
    private Renderer buttonRenderer;

    // Start is called before the first frame update
    void Start()
    {
        buttonRenderer = button.GetComponent<Renderer>();
        buttonRenderer.material.color = Color.gray;
    }

    public void LightButton()
    {
        if (buttonRenderer.material.color != Color.red)
        {
            buttonRenderer.material.color = Color.red;
        }
        else
        {
            buttonRenderer.material.color = Color.white;
        }
    }
}
