using System;

public interface IInput
{
    Action<int> OnScaleChanged { get; set; }

    float Horizontal { get; }
    float Vertical { get; }

    int Scale { get; }
}
