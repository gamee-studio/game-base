using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vector2i = ClipperLib.IntPoint;

public class AwakeCircleClipper : MonoBehaviour, IClip
{
    public DestructibleTerrain terrain;

    public float diameter = 1.2f;

    private float radius = 1.2f;

    public int segmentCount = 10;

    private Vector2 clipPosition;

    public bool CheckBlockOverlapping(Vector2 p, float size)
    {       
        float dx = Mathf.Abs(clipPosition.x - p.x) - radius - size / 2;
        float dy = Mathf.Abs(clipPosition.y - p.y) - radius - size / 2;

        return dx < 0f && dy < 0f;      
    }

    public ClipBounds GetBounds()
    {      
        return new ClipBounds
        {
            lowerPoint = new Vector2(clipPosition.x - radius, clipPosition.y - radius),
            upperPoint = new Vector2(clipPosition.x + radius, clipPosition.y + radius)
        };             
    }

    public List<Vector2i> GetVertices()
    {
        List<Vector2i> vertices = new List<Vector2i>();
        for (int i = 0; i < segmentCount; i++)
        {
            float angle = Mathf.Deg2Rad * (-90f - 360f / segmentCount * i);

            Vector2 point = new Vector2(clipPosition.x + radius * Mathf.Cos(angle), clipPosition.y + radius * Mathf.Sin(angle));
            Vector2i point_i64 = point.ToVector2i();
            vertices.Add(point_i64);
        }
        return vertices;
    }

    void Awake()
    {
        radius = diameter / 2f;
    }

    void Start()
    {
        Vector2 positionWorldSpace = transform.position;
        clipPosition = positionWorldSpace - terrain.GetPositionOffset();

        terrain.ExecuteClip(this);
    }
}
