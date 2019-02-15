using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementConroller : MonoBehaviour, IMovable
{
    public Action OnPositionChanged;

    private IInput input;
    private MoveSpeedModel speedModel;

    public void Init(IInput input)
    {
        speedModel = new MoveSpeedModel(1);

        this.input = input;
    }

    public void Update()
    {
        if (input.Horizontal == 0 && input.Vertical == 0)
            return;

        var direction = new Vector2(input.Horizontal, input.Vertical);
        Move(direction);
    }

    public void Move(Vector2 direction)
    {
        transform.position += new Vector3(direction.x, direction.y, 0) * speedModel.Speed;

        OnPositionChanged?.Invoke();
    }
}
