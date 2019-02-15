using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScaleText scaleText;
    [SerializeField] private RocketText roketText;
    [SerializeField] private Rocket rocketPref;
    [SerializeField] private Asteroid asteroidPref;

    private IInput input;
    private MapController mapController;
    private Rocket rocket;
    private MovementConroller movementConroller;
    private CameraController cameraController;

    private void Awake()
    {
        InitInput();
        InitRocket();
        InitMap();
        InitCamera();
        InitMapDraw();

        InitCanvas();
    }

    private void InitInput()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
            {
                var inputGo = new GameObject("KeyboardInputManager");
                input = inputGo.AddComponent<KeyboardInputManager>();
            }
            break;

            default:
            {
                Debug.LogWarning("Sorry! Developers not made Input!");
            }
            break;
        }
    }

    private void InitRocket()
    {
        rocket = Instantiate(rocketPref);
        rocket.Init(int2.zero, UnityEngine.Random.Range(0, 10000));
        movementConroller = rocket.GetComponent<MovementConroller>();
        movementConroller.Init(input);
    }

    private void InitMap()
    {
        mapController = new MapController(movementConroller, input);
    }

    private void InitCamera()
    {
        cameraController = new CameraController(movementConroller, Camera.main, input);
    }

    private void InitMapDraw()
    {
        var mapDraw = new MapDrawController(movementConroller, mapController, rocket, asteroidPref, input);

    }

    private void InitCanvas()
    {
        scaleText.Init(input);
        roketText.Init(rocket.Rating);
    }
}
