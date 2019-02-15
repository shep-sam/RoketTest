using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;

public class MapController
{
    private MapModel mapModel;
    private MovementConroller movementConroller;
    private IInput input;

    public MapController(MovementConroller movementConroller, IInput input)
    {
        this.movementConroller = movementConroller;
        this.input = input;
        mapModel = new MapModel();

        InitStartMap();
        CheckIfNeedAddChunks();

        movementConroller.OnPositionChanged += OnPositionChanged;
        input.OnScaleChanged += OnScaleChanged;
    }

    ~MapController()
    {
        movementConroller.OnPositionChanged -= OnPositionChanged;
        input.OnScaleChanged = OnScaleChanged;
    }

    private void OnScaleChanged(int scale)
    {
        mapModel.ViewSize = scale;
        CheckIfNeedAddChunks();
    }

    private void OnPositionChanged()
    {
        CheckIfNeedAddChunks();
    }

    private void CheckIfNeedAddChunks()
    {
        if (movementConroller.transform.position.x - mapModel.ViewSize < (mapModel.Border.LeftBorder + 2) * mapModel.ChunkSize)
            AddChunkLeft();

        if (movementConroller.transform.position.x + mapModel.ViewSize > (mapModel.Border.RightBorder - 2) * mapModel.ChunkSize)
            AddChunkRight();

        if (movementConroller.transform.position.y - mapModel.ViewSize < (mapModel.Border.DownBorder + 2) * mapModel.ChunkSize)
            AddChunkDown();

        if (movementConroller.transform.position.y + mapModel.ViewSize > (mapModel.Border.UpBorder - 2) * mapModel.ChunkSize)
            AddChunkUp();
    }

    private void InitStartMap()
    { 
        var width = mapModel.Border.GetWidth();
        var height = mapModel.Border.GetHeight();

        for (int i = mapModel.Border.LeftBorder; i <= mapModel.Border.RightBorder; i++)
        {
            for (int j = mapModel.Border.DownBorder; j <= mapModel.Border.UpBorder; j++)
            {
                var index = new int2(i, j);
                var position = index * mapModel.ChunkSize;

                var chunk = new Chunk(position);

                mapModel.AddChunk(index, chunk);
            }
        }
    }

    public void AddChunkLeft()
    {
        var height = mapModel.Border.GetHeight();
        var newLeftBorder = mapModel.Border.LeftBorder - 1;

        for (int y = mapModel.Border.DownBorder; y <= mapModel.Border.UpBorder; y++)
        {
            var index = new int2(newLeftBorder, y);
            var position = index * mapModel.ChunkSize;

            var chunk = new Chunk(position);

            mapModel.AddChunk(index, chunk);
        }

        mapModel.Border.LeftBorder = newLeftBorder;
    }

    public void AddChunkRight()
    {
        var height = mapModel.Border.GetHeight();
        var newRightBorder = mapModel.Border.RightBorder + 1;

        for (int y = mapModel.Border.DownBorder; y <= mapModel.Border.UpBorder; y++)
        {
            var index = new int2(newRightBorder, y);
            var position = index* mapModel.ChunkSize;

            var chunk = new Chunk(position);

            mapModel.AddChunk(index, chunk);
        }

        mapModel.Border.RightBorder = newRightBorder;
    }

    public void AddChunkUp()
    {
        var width = mapModel.Border.GetWidth();
        var newUpBorder = mapModel.Border.UpBorder + 1;

        for (int x = mapModel.Border.LeftBorder; x <= mapModel.Border.RightBorder; x++)
        {
            var index = new int2(x, newUpBorder);
            var position = index * mapModel.ChunkSize;

            var chunk = new Chunk(position);

            mapModel.AddChunk(index, chunk);
        }

        mapModel.Border.UpBorder = newUpBorder;
    }

    public void AddChunkDown()
    {
        var width = mapModel.Border.GetWidth();
        var newDownBorder = mapModel.Border.DownBorder - 1;

        for (int x = mapModel.Border.LeftBorder; x <= mapModel.Border.RightBorder; x++)
        {
            var index = new int2(x, newDownBorder);
            var position = index * mapModel.ChunkSize;

            var chunk = new Chunk(position);

            mapModel.AddChunk(index, chunk);
        }

        mapModel.Border.DownBorder = newDownBorder;
    }

    public List<Slot> GetSlotsInRange(int2x2 range)
    {
        var list = new List<Slot>();

        var x1 = Mathf.CeilToInt(range.c0.x / mapModel.ChunkSize) - 1;
        var y1 = Mathf.CeilToInt(range.c1.x / mapModel.ChunkSize) - 1;
        var x2 = Mathf.CeilToInt(range.c0.y / mapModel.ChunkSize);
        var y2 = Mathf.CeilToInt(range.c1.y / mapModel.ChunkSize);

        int2x2 indexRange = new int2x2(x1, y1, x2, y2);

        var chunks = GetChunksInRange(indexRange);

        Chunk chunk;
        Slot slot;

        for (int i = 0; i < chunks.Count; i++)
        {
            chunk = chunks[i];

            if (chunk.Position.x > range.c0.x && chunk.Position.x + mapModel.ChunkSize < range.c0.y &&
                chunk.Position.y > range.c1.x && chunk.Position.y + mapModel.ChunkSize < range.c1.y)
            {
                list.AddRange(chunk.Slots);
            }
            else
            {
                for (int j = 0; j < chunk.Slots.Count; j++)
                {
                    slot = chunk.Slots[j];

                    if (slot.Position.x >= range.c0.x && slot.Position.x <= range.c0.y &&
                        slot.Position.y >= range.c1.x && slot.Position.y <= range.c1.y)
                    {
                        list.Add(slot);
                    }
                }
            }
        }
        return list;
    }

    public List<Slot> GetSlotsInRangeAndRating(int2x2 range, int rating)
    {
        var slots = GetSlotsInRange(range);

        return GetNearestToValue(slots, 20);
    }

    private List<Chunk> GetChunksInRange(int2x2 range)
    {
        List<Chunk> chunks = new List<Chunk>();

        int2 index;
        Chunk chunk;

        for (int i = range.c0.x; i <= range.c0.y; i++)
        {
            for (int j = range.c1.x; j <= range.c1.y; j++)
            {  
                index = new int2(i, j);
                chunk = mapModel.GetChunk(index);

                chunks.Add(chunk);
            }
        }

        return chunks;
    }

    private List<Slot> GetNearestToValue(List<Slot> slots, int count)
    {
        var res = slots.OrderBy(x => x.Rating - Math.Abs(count))
            .Take(count).ToList(); ;

        return res;
    }
}
