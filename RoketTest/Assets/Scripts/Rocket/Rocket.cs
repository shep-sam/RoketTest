using Unity.Mathematics;
using UnityEngine;

public class Rocket : MonoBehaviour, IPosition, IRating
{
    private int2 position;
    private int rating;

    public int2 Position => position;
    public int Rating => rating;

    public void Init(int2 position, int rating)
    {
        this.position = position;
        this.rating = rating;
    }
}
