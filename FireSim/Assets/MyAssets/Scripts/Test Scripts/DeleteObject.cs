using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    [SerializeField] private string Tag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tag)
            Destroy(other.gameObject);
    }
}
