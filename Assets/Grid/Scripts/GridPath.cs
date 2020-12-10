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
        GetComponent<LineRenderer>().positionCount = points.Count;
        GetComponent<LineRenderer>().SetPositions
        (
            points.Select(point => new Vector3(point.x, point.y, 0)).ToArray()
        );

        if (color.HasValue)
        {
            GetComponent<LineRenderer>().startColor = color.Value;
            GetComponent<LineRenderer>().endColor = color.Value;
        }
    }

}
