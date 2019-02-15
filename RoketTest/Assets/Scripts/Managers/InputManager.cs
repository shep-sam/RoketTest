using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour, IInput
{
    public Action<int> OnScaleChanged { get; set; }

    protected float horizontal;
    protected float vertical;

    private int scale = 5;

    public float Horizontal { get => horizontal; private set => horizontal = value; }
    public float Vertical { get => vertical; private set => vertical = value; }

    public int Scale 
    { 
        get
        {
            return scale;
        }

        protected set
        {
            if (scale == value)
                return;

            if (value < 5 || value > 10000)
                return;

            scale = value;

            OnScaleChanged?.Invoke(scale);
        }
    }
}
