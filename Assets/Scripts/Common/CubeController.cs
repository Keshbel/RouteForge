using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
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

        _mainCamera.transform.RotateAround(Vector3.zero, Vector3.up, 180);
        currentCube = cubes.Find(cube => cube != currentCube);

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
}
