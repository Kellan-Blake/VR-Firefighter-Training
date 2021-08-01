using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FollowPlayerCamera : MonoBehaviour
{
    private GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Player>().GetComponentInChildren<Camera>().gameObject;
    }

    private void Update()
    {
        transform.position = cam.transform.position;
        transform.rotation = cam.transform.rotation;
    }
}
