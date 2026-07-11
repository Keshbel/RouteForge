using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private List<Vector3> pathList = new List<Vector3>();

    private const float Speed = 5f;

    private GridController _gridController;
    private Tweener _moveTween;
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
        var toPositionNew = new Vector3(toPosition.x, 2, toPosition.z);

        _moveTween?.Kill();
        _moveTween = transform.DOMove(toPositionNew, time).SetEase(Ease.InOutCubic);

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
                float time = distance / Speed;

                yield return MoveStep(pathStep, time);

                _gridController.RemovePathTile(_gridController.GetCellCenterFromWorldPosition(pathStep), true);
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
        const float MaxStepDistanceSqr = 100f;

        for (int i = 0; i < pathList.Count; i++)
        {
            if ((origin - pathList[i]).sqrMagnitude < MaxStepDistanceSqr)
            {
                return i;
            }
        }

        return -1;
    }

    public void StartMove()
    {
        StopAllCoroutines();
        StartCoroutine(MoveAllPath());
    }
}
