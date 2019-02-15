using Unity.Mathematics;
using UnityEngine;

public struct Slot
{
    private int2 position;
    private int rating;

    public int2 Position => position;
    public int Rating => rating;

    public Slot(int2 position, int rating)
    {
        this.position = position;
        this.rating = rating;
    }
}