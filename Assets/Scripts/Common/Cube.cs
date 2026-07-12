using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private const float MinMoveDistanceSqr = 0.0001f;

    [SerializeField] private List<Vector3> pathList = new List<Vector3>();
    [SerializeField, Min(0.1f)] private float movementSpeed = 5f;
    [SerializeField, Min(0f)] private float movementHeight = 2f;
    [SerializeField, Range(0f, 180f)] private float rollAngle = 90f;

    private GridController _gridController;
    private Tweener _moveTween;
    private Tweener _rotationTween;
    private bool _isGoal;
    private bool _isFail;

    public event Action<Cube> MovementEnded;

    public bool IsGoal => _isGoal;

    public bool IsFail => _isFail;

    public void Construct(GridController gridController)
    {
        _gridController = gridController;
    }

    public bool ContainsPathPoint(Vector3 point)
    {
        return pathList.Contains(point);
    }

    public void AddPathPoint(Vector3 point)
    {
        pathList.Add(point);
    }

    /// <summary>
    /// Проверяет, можно ли добавить точку пути без диагонального или пропущенного шага.
    /// </summary>
    /// <param name="point">Мировая позиция центра клетки, которую нужно добавить в путь.</param>
    /// <returns>True, если клетка совпадает с опорной клеткой или находится рядом по прямой.</returns>
    public bool CanAppendPathPoint(Vector3 point)
    {
        if (_gridController == null)
        {
            Debug.LogError("Cube path validation requires GridController dependency.", this);
            return false;
        }

        Vector3Int pointCell = _gridController.GetCellCenterFromWorldPosition(point);
        Vector3Int referenceCell = pathList.Count > 0
            ? _gridController.GetCellCenterFromWorldPosition(pathList[pathList.Count - 1])
            : _gridController.GetCellCenterFromWorldPosition(transform.position);

        return IsSameOrOrthogonalNeighbor(referenceCell, pointCell);
    }

    public void RemovePathPoint(Vector3 point)
    {
        pathList.Remove(point);
    }

    public void MarkGoalReached()
    {
        _isGoal = true;
    }

    private IEnumerator MoveStep(Vector3 toPosition, float time)
    {
        Vector3 targetPosition = new Vector3(toPosition.x, movementHeight, toPosition.z);
        Vector3 moveDirection = targetPosition - transform.position;
        moveDirection.y = 0f;

        _moveTween?.Kill();
        _rotationTween?.Kill();

        _moveTween = transform.DOMove(targetPosition, time).SetEase(Ease.InOutCubic);
        if (moveDirection.sqrMagnitude > MinMoveDistanceSqr && rollAngle > 0f)
        {
            Vector3 rollAxis = Vector3.Cross(Vector3.up, moveDirection.normalized);
            Quaternion targetRotation = Quaternion.AngleAxis(rollAngle, rollAxis) * transform.rotation;
            _rotationTween = transform.DORotateQuaternion(targetRotation, time).SetEase(Ease.InOutCubic);
        }

        yield return _moveTween.WaitForCompletion();
    }

    private IEnumerator MoveAllPath()
    {
        if (_isFail || _isGoal)
        {
            yield break;
        }

        if (_gridController == null)
        {
            Debug.LogError("Cube movement requires GridController dependency.", this);
            yield break;
        }

        int pathListCount = pathList.Count;

        for (int i = 0; i < pathListCount; i++)
        {
            int pathStepIndex = FindNextPathStepIndex(transform.position);

            if (pathStepIndex >= 0)
            {
                Vector3 pathStep = pathList[pathStepIndex];
                float distance = Vector3.Distance(transform.position, pathStep);
                float time = movementSpeed > 0f ? distance / movementSpeed : 0f;

                yield return MoveStep(pathStep, time);

                _gridController.RemovePathTile(_gridController.GetCellCenterFromWorldPosition(pathStep), false);
                pathList.RemoveAt(pathStepIndex);

                if (_gridController.CheckGoal(pathStep, this))
                {
                    for (int j = 0; j < pathList.Count; j++)
                    {
                        _gridController.RemovePathTile(_gridController.GetCellCenterFromWorldPosition(pathList[j]), false);
                    }

                    pathList.Clear();
                    MovementEnded?.Invoke(this);
                    yield break;
                }
            }
        }

        _isFail = true;
        MovementEnded?.Invoke(this);
    }

    private int FindNextPathStepIndex(Vector3 origin)
    {
        Vector3Int originCell = _gridController.GetCellCenterFromWorldPosition(origin);

        for (int i = 0; i < pathList.Count; i++)
        {
            Vector3Int pathCell = _gridController.GetCellCenterFromWorldPosition(pathList[i]);
            if (IsSameOrOrthogonalNeighbor(originCell, pathCell))
            {
                return i;
            }
        }

        return -1;
    }

    private static bool AreOrthogonalNeighbors(Vector3Int first, Vector3Int second)
    {
        int xDistance = Mathf.Abs(first.x - second.x);
        int yDistance = Mathf.Abs(first.y - second.y);
        int zDistance = Mathf.Abs(first.z - second.z);

        return xDistance + yDistance + zDistance == 1;
    }

    private static bool IsSameOrOrthogonalNeighbor(Vector3Int first, Vector3Int second)
    {
        return first == second || AreOrthogonalNeighbors(first, second);
    }

    public void StartMove()
    {
        StopAllCoroutines();
        _moveTween?.Kill();
        _rotationTween?.Kill();
        StartCoroutine(MoveAllPath());
    }

    private void OnDestroy()
    {
        _moveTween?.Kill();
        _rotationTween?.Kill();
    }
}
