using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridPath : MonoBehaviour
{

    /// <summary>
    /// Sets the start and end points of the path.
    /// </summary>
    public void SetPath(List<Vector2> points, Color? color = null)
    {
        // Get the line renderer attached to this gameobject
        LineRenderer renderer = GetComponent<LineRenderer>();

        // Set positions
        renderer.positionCount = points.Count;
        renderer.SetPositions
        (
            points.Select(point => new Vector3(point.x, point.y, 0)).ToArray()
        );

        // Set color
        if (color.HasValue)
        {
            renderer.startColor = color.Value;
            renderer.endColor = color.Value;
        }
    }

}
