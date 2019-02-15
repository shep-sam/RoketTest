using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;

public struct Chunk
{ 
    private int2 position;
    private List<Slot> slots;

    public int2 Position => position;
    public List<Slot> Slots => slots;

    public Chunk(int2 position)
    {
        this.position = position;
        this.slots = new List<Slot>();
        this.slots = FillChunk();
    }

    private List<Slot> FillChunk()
    {
        var totalCount = 25;
        var finalCount = (int)(UnityEngine.Random.Range(30, 100)/100.0f * totalCount);

        slots = new List<Slot>();

        var list = Helper.GetUnrepitedList(totalCount, finalCount);

        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            var posX = item % 5;
            var posY = item / 5;

            var slotPos = position + new int2(posX, posY);
            var slotRating = UnityEngine.Random.Range(0, 10000);

            slots.Add(new Slot(slotPos, slotRating));
        }

        return slots;
    }
}