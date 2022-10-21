using DG.Tweening;
using MessagePack.Formatters;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIEffect : MonoBehaviour
{
    [Header("Data config")]
    public UIEffectType UIEffectType;
    public bool PlayOnAwake = true;
    public float Time = .5f;
    [ShowIf("UIEffectType",UIEffectType.OutBack)] [Header("Outback Effect")]
    [ShowIf("UIEffectType",UIEffectType.OutBack)] public Vector3 FromScale = Vector3.zero;
    [ShowIf("UIEffectType",UIEffectType.OutBack)] [ReadOnly] public Vector3 LocalScale; 
    [ShowIf("UIEffectType",UIEffectType.Shake)] [Header("Shake Effect")]
    [ShowIf("UIEffectType",UIEffectType.Shake)] public float Strength = 3f;
    [ShowIf("UIEffectType", UIEffectType.Move)] [Header("Move Effect")] 
    [ShowIf("UIEffectType", UIEffectType.Move)] public MoveType MoveType;
    [ShowIf("IsShowAttributeFromPosition")] public Vector3 FromPosition;
    [ShowIf("UIEffectType", UIEffectType.Move)] [ReadOnly] public Vector3 LocalPostion; 
    [ShowIf("MoveType", MoveType.Direction)] public DirectionType DirectionType;
    [ShowIf("MoveType", MoveType.Direction)] public float Offset;
    private Sequence sequence;

    private bool IsShowAttributeFromPosition => UIEffectType == UIEffectType.Move && MoveType == MoveType.Vector3;

    public void Awake()
    {
        LocalPostion = transform.localPosition;
        LocalScale = transform.localScale;
    }

    public void OnEnable()
    {
        if (PlayOnAwake)
        {
            PlayAnim();
        }
    }

    public void PlayAnim()
    {
        switch (UIEffectType)
        {
            case UIEffectType.OutBack:
                sequence = DOTween.Sequence().OnStart(() => transform.localScale = FromScale)
                    .Append(transform.DOScale(Vector3.one, Time).OnKill(()=>transform.localScale = LocalScale).SetEase(Ease.OutBack));
                break;
            case UIEffectType.Shake:
                sequence = DOTween.Sequence().Append(transform.DOShakeRotation(Time, Strength).SetEase(Ease.Linear));
                break;
            case UIEffectType.Move:
                switch (MoveType)
                {
                    case MoveType.Vector3:
                        transform.localPosition = FromPosition;
                        transform.DOMove(LocalPostion, Time).SetEase(Ease.Linear);
                        break;
                    case MoveType.Direction:
                        
                        break;
                }

                break;
        }
    }

    public void OnDisable()
    {
        sequence?.Kill();
    }
}

public enum UIEffectType
{
    OutBack,
    Shake,
    Move,
}

public enum MoveType
{
    Vector3,
    Direction,
}

public enum DirectionType
{
    Up,
    Down,
    Left,
    Right,
}