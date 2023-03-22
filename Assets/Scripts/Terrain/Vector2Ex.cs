using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using int64 = System.Int64;
using uint32 = System.UInt32;
using Vector2i = ClipperLib.IntPoint;
using Vector2f = UnityEngine.Vector2;

public static class VectorEx
{
    public static float float2int64 = 100000.0f;

    public static Vector2i ToVector2i(this Vector2f p)
    {
        return new Vector2i
        {
            x = (int64)(p.x * float2int64),
            y = (int64)(p.y * float2int64)
        };
    }

    public static Vector3 ToVector3f(this Vector2f p)
    {
        return new Vector3
        {
            x = p.x,
            y = p.y,
            z = 0f
        };
    }

    public static Vector2f ToVector2f(this Vector2i p)
    {
        return new Vector2f
        {
            x = (float)(p.x / float2int64),
            y = (float)(p.y / float2int64)
        };
    }

    public static Vector3 ToVector3f(this Vector2i p)
    {
        return new Vector3
        {
            x = (float)(p.x / float2int64),
            y = (float)(p.y / float2int64),
            z = 0f
        };
    }

    public static float Cross(Vector2f A, Vector2f B)
    {
        float m = A.x * B.y - A.y * B.x;
        return m;
    }

    public static Vector2f Cross(Vector2 A, float s)
    {
        return new Vector2f { x = -A.y * s, y = A.x * s };
    }

    public struct Line // ax + by + c = 0
    {
        public float a;
        public float b;
        public float c;
        public float angle;

        public Line(Vector2i p1, Vector2i p2)
        {
            Vector2i d = new Vector2i { x = p2.x - p1.x, y = p2.y - p1.y };
            float m = Mathf.Sqrt(d.x * d.x + d.y * d.y);
            a = -d.y / m;
            b = d.x / m;
            c = -(a * p1.x + b * p1.y);
            angle = 0f;
        }

        public Line(Vector2 p1, Vector2 p2)
        {
            Vector2 d = p2 - p1;
            float m = d.magnitude;
            a = -d.y / m;
            b = d.x / m;
            c = -(a * p1.x + b * p1.y);
            angle = Mathf.Rad2Deg * Mathf.Atan2(-a, b);
        }

        public float GetDistance(Vector2i p)
        {
            return Mathf.Abs(a * p.x + b * p.y + c);
        }

        public float GetDistance(Vector2 p)
        {
            return Mathf.Abs(a * p.x + b * p.y + c);
        }
    }
}
