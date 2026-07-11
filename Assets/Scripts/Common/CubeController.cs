using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private static readonly Vector3 CameraPivot = new Vector3(0f, 0f, 0f);

    [Space]
    [SerializeField] private Cube currentCube;
    [Space]
    [SerializeField] private List<Cube> cubes = new List<Cube>();

    private GameManager _gameManager;
    private GridController _gridController;
    private Camera _mainCamera;

    public Cube CurrentCube => currentCube;

    public int CubeCount => cubes.Count;

    public void Construct(GameManager gameManager, GridController gridController, Camera mainCamera)
    {
        _gameManager = gameManager;
        _gridController = gridController;
        _mainCamera = mainCamera;

        foreach (var cube in cubes)
        {
            cube.Construct(_gridController);
        }
    }

    private void OnEnable()
    {
        foreach (var cube in cubes)
        {
            cube.MovementEnded += CheckEnding;
        }
    }

    private void OnDestroy()
    {
        foreach (var cube in cubes)
        {
            cube.MovementEnded -= CheckEnding;
        }
    }

    public void CheckEnding(Cube completedCube)
    {
        var countGoal = 0;
        var countFail = 0;

        foreach (var cube in cubes)
        {
            if (cube.IsGoal)
            {
                countGoal++;
            }

            if (cube.IsFail)
            {
                countFail++;
            }
        }

        if (countGoal + countFail >= cubes.Count)
        {
            _gameManager.EndGame(countGoal);
        }
    }

    public void ChangeCurrentCube()
    {
        if (_mainCamera == null)
        {
            Debug.LogError("CubeController requires an explicit camera reference.", this);
            return;
        }

        _mainCamera.transform.RotateAround(CameraPivot, Vector3.up, 180);
        currentCube = FindNextCube();

        foreach (var cube in cubes)
        {
            cube.transform.Rotate(0, 180, 0, Space.Self);
        }
    }

    public void StartMove()
    {
        if (_gameManager == null || !_gameManager.StartRunning())
        {
            return;
        }

        foreach (var cube in cubes)
        {
            cube.StartMove();
        }
    }

    private Cube FindNextCube()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i] != currentCube)
            {
                return cubes[i];
            }
        }

        return currentCube;
    }
}
