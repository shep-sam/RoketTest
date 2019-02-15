using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

public class MapDrawController
{
    private List<Asteroid> asteroids = new List<Asteroid>();
    private int maxAsteroidsCount = 100;

    public Vector2 position;
    public int2 size;

    private MovementConroller movement;
    private MapController mapController;
    private Rocket rocket;
    private IInput input;

    private bool isSuper;

    public MapDrawController(MovementConroller movement, MapController mapController,Rocket rocket, Asteroid asteroidPref, IInput input)
    {
        this.movement = movement;
        this.mapController = mapController;
        this.rocket = rocket;
        this.input = input;

        size = new int2(1, 1) * input.Scale;

        movement.OnPositionChanged += OnPositionChanged;
        input.OnScaleChanged += OnScaleChanged;

        InitAteroids(asteroidPref);

        Draw();
    }

    ~MapDrawController()
    {
        movement.OnPositionChanged -= OnPositionChanged;
        input.OnScaleChanged -= OnScaleChanged;
    }

    private void InitAteroids(Asteroid asteroidPref)
    {
        for (int i = 0; i < maxAsteroidsCount; i++)
        {
            var asteroid = UnityEngine.Object.Instantiate(asteroidPref);
            asteroid.gameObject.SetActive(false);
            asteroids.Add(asteroid);
        }
    }

    private void OnPositionChanged()
    {
        if (size.x < 10)
            Draw();
        else
            SuperDraw();
    }

    private void OnScaleChanged(int scale)
    {
        size = new int2(1,1) * input.Scale;

        if (scale == 10)
            ResurveAsteroids();

        if (scale < 10)
            Draw();
        else
            SuperDraw();
    }

    private void ResurveAsteroids()
    {
        for(int i = 0; i < asteroids.Count; i++)
        {
            if (i < 20)
                asteroids[i].gameObject.SetActive(true);
            else
                asteroids[i].gameObject.SetActive(false);
        }
    }

    public void Draw()
    {
        position = movement.transform.position;

        var leftDownCorner = position - Vector2.one * Mathf.FloorToInt(size.x / 2.0f);
        var rightUpCorner = leftDownCorner + Vector2.one * (size.y - 1);

        int2x2 range = new int2x2((int)leftDownCorner.x, (int)leftDownCorner.y, (int)rightUpCorner.x, (int)rightUpCorner.y);

        var slots = mapController.GetSlotsInRange(range);

        foreach (var asteroid in asteroids)
            asteroid.gameObject.SetActive(false);

        for (int i = 0; i < slots.Count; i++)
        {
            var asteroid = asteroids[i];
            asteroid.SetData(slots[i]);
            asteroid.gameObject.SetActive(true);
        }
    }

    public void SuperDraw()
    {
        position = movement.transform.position;

        var leftDownCorner = position - Vector2.one * Mathf.FloorToInt(size.x / 2.0f);
        var rightUpCorner = leftDownCorner + Vector2.one * (size.y - 1);

        int2x2 range = new int2x2((int)leftDownCorner.x, (int)leftDownCorner.y, (int)rightUpCorner.x, (int)rightUpCorner.y);

        var slots = mapController.GetSlotsInRangeAndRating(range, rocket.Rating);

        for (int i = 0; i < slots.Count; i++)
        {
            var asteroid = asteroids[i];
            asteroid.SetData(slots[i]);
        }
    }
}
