using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using int64 = System.Int64;
using Vector2i = ClipperLib.IntPoint;
using Vector2f = UnityEngine.Vector2;

public static class BlockSimplification
{
    public static int64 epsilon;

    public static Vector2i currentLowerPoint;

    public static Vector2i currentUpperPoint;

    public static int GetMask(Vector2i p)
    {
        int mask = 0;

        if (p.x == currentLowerPoint.x)
            mask = mask | 1;

        if (p.x == currentUpperPoint.x)
            mask = mask | 2;

        if (p.y == currentLowerPoint.y)
            mask = mask | 4;

        if (p.y == currentUpperPoint.y)
            mask = mask | 8;

        return mask;
    }

    public static bool PointNotLieOnEdges(Vector2i p)
    {
        //return p.x != currentLowerPoint.x && p.x != currentUpperPoint.x
        //    && p.y != currentLowerPoint.y && p.y != currentUpperPoint.y;

        return p.x - currentLowerPoint.x - epsilon > 0 && p.y - currentLowerPoint.y - epsilon > 0
            && currentUpperPoint.x - p.x - epsilon > 0 && currentUpperPoint.y - p.y - epsilon > 0;
    }

    public static void RamerDouglasPeucker(List<Vector2i> inPolygon, int[] mask, int a, int b, ref int removeCount)
    {
        if (b - a < 2)
            return;

        int begin = a + 1;
        int end = b - 1;
        int index = 0;
        float dmax = 0f;
        VectorEx.Line line = new VectorEx.Line(inPolygon[a], inPolygon[b]);

        for (int i = begin; i <= end; i++)
        {
            float d = line.GetDistance(inPolygon[i]);

            if (d > dmax)
            {
                index = i;
                dmax = d;
            }
        }

        if (dmax > epsilon)
        {
            RamerDouglasPeucker(inPolygon, mask, a, index, ref removeCount);
            RamerDouglasPeucker(inPolygon, mask, index, b, ref removeCount);
        }
        else
        {

            //int maskA = GetMask(inPolygon[a]);
            //int maskB = GetMask(inPolygon[b]);

            //if ((maskA & maskB) != 0)
            //{
            //    return;
            //}

            for (int i = begin; i <= end; i++)
            {
                if (PointNotLieOnEdges(inPolygon[i]))
                {
                    mask[i] = 1;
                    removeCount++;
                }
            }
        }
    }

    public static Vector2i[] Execute(List<Vector2i> inPolygon, List<List<Vector2f>> outEdges)
    {
        int inVertexCount = inPolygon.Count;

        if (inVertexCount < 2)
		    return null;

        int[] polygonMask = new int[inVertexCount];

        int removeCount = 0;
        if (epsilon > 0f)
        {
            RamerDouglasPeucker(inPolygon, polygonMask, 0, inVertexCount - 1, ref removeCount);
        }
        
        Vector2i[] outPolygon = new Vector2i[inVertexCount - removeCount];

        List<Vector2f> outEdge = new List<Vector2f>();

        int[] edgeMask = new int[outPolygon.Length];

        int j = 0;
        for (int i = 0; i < inVertexCount; i++)
	    {
            if (polygonMask[i] != 1)
            {               
			    outPolygon[j] = inPolygon[i];	    
			    edgeMask[j] = GetMask(inPolygon[i]);
			    j++;
		    }
	    }

        int maskPrev, mask, a;
        for (int i = 0; i < edgeMask.Length; i++)
        {
            maskPrev = edgeMask[(i - 1 + edgeMask.Length) % edgeMask.Length];
            mask = edgeMask[i];
            a = mask & maskPrev;

            if (mask != 0 && a != 0)
            {
                if (outEdge.Count > 1)
                {
                    outEdges.Add(outEdge);
                    outEdge = new List<Vector2f>();
                }
                else
                {
                    outEdge.Clear();
                }

                outEdge.Add(outPolygon[i].ToVector2f());
            }
            else
            {
                outEdge.Add(outPolygon[i].ToVector2f());
            }
        }

        maskPrev = edgeMask[edgeMask.Length - 1];
        mask = edgeMask[0];
        a = mask & maskPrev;
   
        if (mask != 0 && a != 0)
        {
            if (outEdge.Count > 1)
            {
                outEdges.Add(outEdge);
                outEdge = new List<Vector2f>();
            }
            else
            {
                outEdge.Clear();
            }

            outEdge.Add(outPolygon[0].ToVector2f());
        }
        else
        {
            outEdge.Add(outPolygon[0].ToVector2f());
        }

        if (outEdge.Count > 1)
        {
            outEdges.Add(outEdge);
        }

        return outPolygon;
    }
}
