using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Pancake;
using UnityEngine;

[DeclareFoldoutGroup("Expansion Settings", Title = "Expansion Settings", Expanded = true)]
public class MoveObject : MonoBehaviour
{
    public GameObject TargetMoveGO;
    private bool isRun = true;

    public bool IsRun
    {
        get => isRun;
        set
        {
            OnIsRunChanged(value);
            isRun = value;
        }
    }
    
    [Range(0f,1000f)]public float Speed = 5f;
    [Range(0f,10f)]public float DelayStart;
    [Range(0f,10f)]public float DelayNextMove;
    [Range(0f,10f)]public float DelayRotate;
    [Group("Expansion Settings")]public bool IsStartAtFirstPosition = true;
    [Group("Expansion Settings")]public bool IsPlayOnAwake = true;
    [Group("Expansion Settings")]public bool IsLoop = true;
    [Group("Expansion Settings")]public bool IsLookAtTarget = true;

    [Header("The path must be at least 2 points.", order=1)]
    public List<Transform> Path;

    private Queue<Vector3> PathQueue = new Queue<Vector3>();
    private Vector3 nextPoint;
    private TweenerCore<Vector3,Vector3,VectorOptions> sequence;
    private Sequence nextSequence;
    
    void Start()
    {
        foreach (Transform item in Path)
        {
            PathQueue.Enqueue(item.position);
        }

        if (PathQueue.Count < 2) return;

        if (IsStartAtFirstPosition) TargetMoveGO.transform.position = PathQueue.Peek();

        if (IsPlayOnAwake)
        {
            nextSequence = DOTween.Sequence().AppendInterval(DelayStart).AppendCallback(()=>StartMove());
        }
    }

    private void OnDestroy()
    {
        sequence?.Kill();
        nextSequence?.Kill();
    }

    public void StartMove()
    {
        Vector3 tempPoint = PathQueue.Peek();
        PathQueue.Dequeue();
        PathQueue.Enqueue(tempPoint);
        nextPoint = PathQueue.Peek();
        
        if (IsLookAtTarget) TargetMoveGO.transform.DOLookAt(nextPoint, DelayRotate);
        sequence = TargetMoveGO.transform.DOMove(nextPoint, Speed).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(
        () =>
        {
            if (IsLoop)
            {
                nextSequence?.Play();
            }
        }).OnStart(() =>
        {
            nextSequence = DOTween.Sequence().AppendInterval(DelayNextMove).AppendCallback(() => StartMove());
            nextSequence.Pause();
        });
    }

    public void OnIsRunChanged(bool value)
    {
        if (value) sequence.Play();
        else sequence.Pause();
    }

    private void OnDrawGizmos()
    {
        if (!Path.IsNullOrEmpty())
        {
            try
            {
                foreach (Transform item in Path)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(item.transform.position, 0.2f);
                }
            }
            catch (Exception e)
            {
                Path = new List<Transform>();
                throw;
            }
        }
    }
}