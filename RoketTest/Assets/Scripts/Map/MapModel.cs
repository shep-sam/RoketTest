using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;

public class MapModel
{
    private Border border;
    private int chunkSize;
    private int viewSize;
    private Dictionary<int2, Chunk> chunks;

    public Border Border { get => border; private set => border = value; }
    public Dictionary<int2, Chunk> Chunks { get => chunks; private set => chunks = value; }
    public int ChunkSize { get => chunkSize; set => chunkSize = value; }
    public int ViewSize { get => viewSize; set => viewSize = value; }

    public MapModel()
    {
        chunks = new Dictionary<int2, Chunk>();

        chunkSize = 5;
        viewSize = 5;
        border = new Border
        {
            LeftBorder = -2,
            RightBorder = 2,
            DownBorder = -2,
            UpBorder = 2
        };
    }

    public void AddChunk(int2 index, Chunk chunk)
    {
        chunks.Add(index, chunk);
    }

    public Chunk GetChunk(int2 index)
    {
        Chunk chunk;
        chunks.TryGetValue(index, out chunk);

        return chunks[index];
    }
}

public class Border
{
    public int LeftBorder;
    public int RightBorder;
    public int UpBorder;
    public int DownBorder;

    public int GetWidth()
    {
        return RightBorder - LeftBorder;
    }

    public int GetHeight()
    {
        return UpBorder - DownBorder;
    }
}