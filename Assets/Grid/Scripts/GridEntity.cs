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
        Color.RGBToHSV(color, out float h, out float s, out float v);
        color = Color.HSVToRGB(h, s, enableEmission ? 1f : v);
        renderer.material.color = color;
    }

}
