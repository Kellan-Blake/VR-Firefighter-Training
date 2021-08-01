using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    [SerializeField] private GameObject toInstantiate;
    [SerializeField] private Transform objectPosition;

    public void CreateObject()
    {
        if (!objectPosition)
            Instantiate(toInstantiate, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion());
        else
            Instantiate(toInstantiate, objectPosition.position, objectPosition.rotation);
    }
}
