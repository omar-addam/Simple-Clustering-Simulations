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

    /// <summary>
    /// Draws a circle around a point.
    /// </summary>
    public void SetCircle(Vector2 center, float radius, Color color)
    {
        // Get the line renderer attached to this gameobject
        LineRenderer renderer = GetComponent<LineRenderer>();

        // Initialize parameters
        var segments = 360;
        var pointCount = segments + 1;
        var points = new Vector2[pointCount];

        // Compute circle points
        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector2(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius);
        }

        // Set positions
        renderer.positionCount = pointCount;
        renderer.SetPositions
        (
            points.Select(point => new Vector3(point.x, point.y, 0)).ToArray()
        );

        // Set color
        renderer.startColor = color;
        renderer.endColor = color;
    }

}
