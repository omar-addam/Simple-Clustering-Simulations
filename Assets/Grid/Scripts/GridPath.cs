using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPath : MonoBehaviour
{

    /// <summary>
    /// Sets the start and end points of the path.
    /// </summary>
    public void SetPath(Vector2 start, Vector2 end)
    {
        GetComponent<LineRenderer>().SetPositions
        (
            new Vector3[]
            {
                new Vector3(start.x, start.y, 0),
                new Vector3(end.x, end.y, 0)
            }
        );
    }

}
