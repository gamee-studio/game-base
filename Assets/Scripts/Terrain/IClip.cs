using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector2i = ClipperLib.IntPoint;

public struct ClipBounds
{
    public Vector2 lowerPoint;
    public Vector2 upperPoint;
}

public interface IClip
{
    ClipBounds GetBounds();

    List<Vector2i> GetVertices();

    bool CheckBlockOverlapping(Vector2 p, float size);
}
