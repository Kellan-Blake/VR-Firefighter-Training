/*
 * This VR Controller uses elements found from a video on Youtube Published by "Valem"
 * link is here https://www.youtube.com/watch?v=5C6zr4Q5AlA
 */
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRController : MonoBehaviour
{

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private SteamVR_Action_Vector2 input = null;
    [SerializeField] private GameObject VRCamera;
    private CharacterController characterController = null;
    private float y;


    private void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        y = characterController.center.y;
    }

    private void Update()
    {
        if (input.axis.magnitude > sensitivity)
        {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
            characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0, 9.81f, 0) * Time.deltaTime);
        }
        Vector3 newCenter = new Vector3(VRCamera.transform.localPosition.x, y, VRCamera.transform.localPosition.z);
        Vector3 forward = VRCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        newCenter -= forward * .2f;
        characterController.center = newCenter;
    }


}
