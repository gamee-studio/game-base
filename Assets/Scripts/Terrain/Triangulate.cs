using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using int64 = System.Int64;
using uint32 = System.UInt32;
using Vector2i = ClipperLib.IntPoint;
using Vector2f = UnityEngine.Vector2;

public static class Triangulate 
{
    public static void Execute(Vector3[] vertices, int begin, int end, List<int> triangles)
    {
        int size = end - begin;
        if (size == 3)
        {
            triangles.Add(0 + begin);
            triangles.Add(1 + begin);
            triangles.Add(2 + begin);
        }
        else if (size > 3)
        {
            int remainingCount = size;
            bool[] visited = new bool[size];

            int a = 0 + begin;
            int b = 1 + begin;
            int c = 2 + begin;

            int it = size * 3;
            while (it >= 0)
            {
                it--;
                bool validTriangle = true;

                if (!TriangleIsClockwise(vertices[a], vertices[b], vertices[c]))
                {
                    validTriangle = false;
                }

                if (validTriangle)
                {
                    for (int i = 0; i < size; i++)
                    {
                        int idx = i + begin;
                        if (visited[i] == false && idx != a && idx != b && idx != c)
                        {
                            if (TriangleContainsPoint(vertices[a], vertices[b], vertices[c], vertices[idx]))
                            {
                                validTriangle = false;
                                break;
                            }
                        }
                    }
                }

                if (validTriangle)
                {
                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(c);

                    if (remainingCount == 3)
                    {
                        break;
                    }

                    visited[b - begin] = true;
                    remainingCount--;

                    b = c;
                }
                else
                {
                    a = b;
                    b = c;
                }

                for (int i = 0; i < size; i++)
                {
                    c = c + 1;
                    if (c >= end)
                        c = begin;
                    if (!visited[c - begin])
                    {
                        break;
                    }
                }
            }
        }
    }

    public static bool TriangleContainsPoint(Vector2f A, Vector2f B, Vector2f C, Vector2f P)
    {
        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;

        ax = C.x - B.x; ay = C.y - B.y;
        bx = A.x - C.x; by = A.y - C.y;
        cx = B.x - A.x; cy = B.y - A.y;
        apx = P.x - A.x; apy = P.y - A.y;
        bpx = P.x - B.x; bpy = P.y - B.y;
        cpx = P.x - C.x; cpy = P.y - C.y;

        float C1 = ax * bpy - ay * bpx;
        float C2 = cx * apy - cy * apx;
        float C3 = bx * cpy - by * cpx;

        return (C1 > 0f && C2 > 0f && C3 > 0f) || (C1 < 0f && C2 < 0f && C3 < 0f);
    }

    public static bool TriangleIsClockwise(Vector2f A, Vector2f B, Vector2f C)
    {
        return (B.x - A.x) * (C.y - A.y) - (B.y - A.y) * (C.x - A.x) <= 0f;
    }
}
