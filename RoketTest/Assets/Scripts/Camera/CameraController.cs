using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController
{
    private Camera camera;
    private MovementConroller target;
    private IInput input;

    public CameraController(MovementConroller target, Camera camera, IInput input)
    {
        this.camera = camera;
        this.target = target;

        target.OnPositionChanged += OnPositionChanged;
        input.OnScaleChanged += OnScaleChanged;
    }

    ~CameraController()
    {
        target.OnPositionChanged -= OnPositionChanged;
        input.OnScaleChanged -= OnScaleChanged;
    }

    private void OnScaleChanged(int scale)
    {
        camera.orthographicSize = scale;
    }

    private void OnPositionChanged()
    {
        camera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }
}
