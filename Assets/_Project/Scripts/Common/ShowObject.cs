using System;
using System.Collections.Generic;
using System.Web.WebPages;
using DG.Tweening;
using Pancake;
using UnityEngine;

[DeclareHorizontalGroup("horizontal")]
[DeclareVerticalGroup("horizontal/vars")]
[DeclareVerticalGroup("horizontal/buttons")]
public class ShowObject : MonoBehaviour
{
    public bool IsShowByTesting;
    public bool IsShowByLevel;
    public bool IsShowByTime;
    public float DelayShowTime;
    [ShowIf(nameof(IsShowByLevel))] public List<int> LevelsShow;
    [ShowIf("IsShowByTime")] public int MaxTimeShow;
    [ShowIf("IsShowByTime")][Group("horizontal/vars")][ReadOnly] public string ShowID;
    
    [ShowIf("IsShowByTime")][Button, Group("horizontal/buttons")]
    public void RandomShowID()
    {
        if (ShowID == null || ShowID.IsEmpty())
        {
            ShowID = Ulid.NewUlid().ToString();
        }
    }

    private bool IsLevelInLevelsShow()
    {
        foreach (int item in LevelsShow)
        {
            if (Data.CurrentLevel == item)
            {
                return true;
            }
        }

        return false;
    }
    
    private bool EnableToShow()
    {
        bool testingCondition = !IsShowByTesting || (IsShowByTesting && Data.IsTesting);
        bool levelCondition = !IsShowByLevel || (IsShowByLevel && IsLevelInLevelsShow());
        bool timeCondition = !IsShowByTime || (IsShowByTime && Data.GetNumberShowGameObject(ShowID) <= MaxTimeShow);
        return testingCondition && levelCondition && timeCondition;
    }

    public void Awake()
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
        DOTween.Sequence().AppendInterval(DelayShowTime).AppendCallback(()=>
        {
            if (IsShowByTime) Data.IncreaseNumberShowGameObject(ShowID);
            gameObject.SetActive(EnableToShow());
        });
    }
}
