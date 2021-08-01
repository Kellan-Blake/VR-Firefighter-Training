using System.Collections.Generic;
using UnityEngine;

public class MaterialBlend : MonoBehaviour
{
    [SerializeField] private Material material1;
    [SerializeField] private Material material2;
    [SerializeField] private List<MeshRenderer> mesh = new List<MeshRenderer>();
    [SerializeField] private Lever lever;
    [SerializeField] private HandCrank crank;

    public void SetBlend()
    {
        float blend = 0;
        if (lever != null)
            blend = lever.GetAngleValue();
        else if (crank != null)
            blend = crank.GetAngleValue();
        for(int i = 0; i < mesh.Count; i++)
        {
            mesh[i].material.Lerp(material1, material2, blend);
        }
    }
}
