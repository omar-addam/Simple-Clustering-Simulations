using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    
    /// <summary>
    /// Sets the color of the entity.
    /// </summary>
    public void SetColor(Color color)
    {
        // Create a clone of the material
        GetComponent<Renderer>().material = new Material(GetComponent<Renderer>().material);

        // Set the color
        GetComponent<Renderer>().material.color = color;

        // Set emission color
        GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

}
