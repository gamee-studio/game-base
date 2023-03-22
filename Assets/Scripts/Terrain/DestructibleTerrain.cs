using System.Collections.Generic;
using UnityEngine;
using Vector2f = UnityEngine.Vector2;
using Vector2i = ClipperLib.IntPoint;

using int64 = System.Int64;

public class DestructibleTerrain : MonoBehaviour
{
    public Material material;

    [Range(0.5f, 1.0f)]
    public float blockSize;

    private int64 blockSizeScaled;

    [Range(0.0f, 5.0f)]
    public float simplifyEpsilonPercent;

    [Range(1, 100)]
    public int resolutionX = 10;

    [Range(1, 100)]
    public int resolutionY = 10;

    public float depth = 1.0f;

    private float width;

    private float height;

    private DestructibleBlock[] blocks;

    private void Awake()
    {
        BlockSimplification.epsilon = (int64)(simplifyEpsilonPercent / 100f * blockSize * VectorEx.float2int64);

        width = blockSize * resolutionX;
        height = blockSize * resolutionY;
        blockSizeScaled = (int64)(blockSize * VectorEx.float2int64);

        Initialize();
    }

    public void Initialize()
    {
        blocks = new DestructibleBlock[resolutionX * resolutionY];

        for (int x = 0; x < resolutionX; x++)
        {
            for (int y = 0; y < resolutionY; y++)
            {
                List<List<Vector2i>> polygons = new List<List<Vector2i>>();

                List<Vector2i> vertices = new List<Vector2i>();
                vertices.Add(new Vector2i { x = x * blockSizeScaled, y = (y + 1) * blockSizeScaled });
                vertices.Add(new Vector2i { x = x * blockSizeScaled, y = y * blockSizeScaled });
                vertices.Add(new Vector2i { x = (x + 1) * blockSizeScaled, y = y * blockSizeScaled });
                vertices.Add(new Vector2i { x = (x + 1) * blockSizeScaled, y = (y + 1) * blockSizeScaled });

                polygons.Add(vertices);

                int idx = x + resolutionX * y;

                DestructibleBlock block = CreateBlock();
                blocks[idx] = block;

                UpdateBlockBounds(x, y);

                block.UpdateGeometryWithMoreVertices(polygons, width, height, depth);
            }
        }
    }

    public Vector2 GetPositionOffset()
    {
        return transform.position;
    }

    private DestructibleBlock CreateBlock()
    {
        GameObject childObject = new GameObject();
        childObject.name = "DestructableBlock";
        childObject.transform.SetParent(transform);
        childObject.transform.localPosition = Vector3.zero;

        DestructibleBlock blockComp = childObject.AddComponent<DestructibleBlock>();
        blockComp.SetMaterial(material);

        return blockComp;
    }

    private void UpdateBlockBounds(int x, int y)
    {
        int lx = x;
        int ly = y;
        int ux = x + 1;
        int uy = y + 1;

        if (lx == 0) lx = -1;
        if (ly == 0) ly = -1;
        if (ux == resolutionX) ux = resolutionX + 1;
        if (uy == resolutionY) uy = resolutionY + 1;

        BlockSimplification.currentLowerPoint = new Vector2i
        {
            x = lx * blockSizeScaled,
            y = ly * blockSizeScaled
        };

        BlockSimplification.currentUpperPoint = new Vector2i
        {
            x = ux * blockSizeScaled,
            y = uy * blockSizeScaled
        };
    }

    public void ExecuteClip(IClip clip)
    {
        BlockSimplification.epsilon = (int64)(simplifyEpsilonPercent / 100f * blockSize * VectorEx.float2int64);

        List<Vector2i> clipVertices = clip.GetVertices();

        ClipBounds bounds = clip.GetBounds();
        int x1 = Mathf.Max(0, (int)(bounds.lowerPoint.x / blockSize));
        if (x1 > resolutionX - 1) return;
        int y1 = Mathf.Max(0, (int)(bounds.lowerPoint.y / blockSize));
        if (y1 > resolutionY - 1) return;
        int x2 = Mathf.Min(resolutionX - 1, (int)(bounds.upperPoint.x / blockSize));
        if (x2 < 0) return;
        int y2 = Mathf.Min(resolutionY - 1, (int)(bounds.upperPoint.y / blockSize));
        if (y2 < 0) return;

        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                if (clip.CheckBlockOverlapping(new Vector2f((x + 0.5f) * blockSize, (y + 0.5f) * blockSize), blockSize))
                {
                    DestructibleBlock block = blocks[x + resolutionX * y];

                    List<List<Vector2i>> solutions = new List<List<Vector2i>>();

                    ClipperLib.Clipper clipper = new ClipperLib.Clipper();
                    clipper.AddPolygons(block.Polygons, ClipperLib.PolyType.ptSubject);
                    clipper.AddPolygon(clipVertices, ClipperLib.PolyType.ptClip);
                    clipper.Execute(ClipperLib.ClipType.ctDifference, solutions,
                        ClipperLib.PolyFillType.pftNonZero, ClipperLib.PolyFillType.pftNonZero);

                    UpdateBlockBounds(x, y);

                    block.UpdateGeometryWithMoreVertices(solutions, width, height, depth);
                }
                
            }
        }      
    }
}
