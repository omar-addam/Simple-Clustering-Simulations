using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    
    /// <summary>
    /// Sets the color of the entity.
    /// </summary>
    public void SetColor(Color color, bool enableEmission = true)
    {
        // Get the renderer attached to this gameobject
        Renderer renderer = GetComponent<Renderer>();

        // Create a clone of the material (this will ensure that each object can have its own color)
        renderer.material = new Material(GetComponent<Renderer>().material);

        // Set the color
        renderer.material.color = color;

        // Set emission color
        if (enableEmission)
        {
            renderer.material.SetColor("_EmissionColor", color);
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

}
