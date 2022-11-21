using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ShowObject : MonoBehaviour
{
    public bool IsShowByTesting;
    public bool IsShowByLevel;
    public float DelayShowTime;

    [ShowIf("IsShowByLevel")] public int LevelShow;
    private bool EnableToShow()
    {
        bool levelCondition = LevelShow == Data.CurrentLevel;
        bool testingCondition = Data.IsTesting;
        return (IsShowByTesting && testingCondition) || (IsShowByLevel && levelCondition);
    }

    void Start()
    {
        Setup();
        
        if (IsShowByLevel) EventController.CurrentLevelChanged += Setup;
        if (IsShowByTesting) EventController.OnDebugChanged += Setup;
    }
    
    private void OnDestroy()
    {
        if (IsShowByLevel) EventController.CurrentLevelChanged -= Setup;
        if (IsShowByTesting) EventController.OnDebugChanged -= Setup;
    }

    public void Setup()
    {
        if (DelayShowTime>0) gameObject.SetActive(false);
        DOTween.Sequence().AppendInterval(DelayShowTime).AppendCallback(()=>gameObject.SetActive(EnableToShow()));
    }
}
