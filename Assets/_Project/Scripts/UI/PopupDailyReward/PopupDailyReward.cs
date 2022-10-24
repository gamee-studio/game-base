using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;

public class PopupDailyReward : Popup
{
    public GameObject BtnWatchVideo;
    public GameObject BtnClaim;

    [ReadOnly] public DailyRewardItem CurrentItem;
    public List<DailyRewardItem> DailyRewardItems => GetComponentsInChildren<DailyRewardItem>().ToList();
    
    
    private Sequence sequence;

    protected override void BeforeShow()
    {
        base.BeforeShow();
        PopupController.Instance.Show<PopupUI>();
        ResetDailyReward();
        Setup();
    }

    public void ResetDailyReward()
    {
        if (!Data.IsClaimedTodayDailyReward() && Data.DailyRewardDayIndex == 29)
        {
            Data.DailyRewardDayIndex = 1;
            Data.IsStartLoopingDailyReward = true;
        }
    }

    protected override void AfterHidden()
    {
        base.AfterHidden();
        //PopupController.Instance.HideAll();
        //PopupController.Instance.Show<PopupHome>();
        if (!PopupController.Instance.Get<PopupHome>().isActiveAndEnabled)
        {
            GameManager.Instance.GameState = GameState.PlayingGame;
            PopupController.Instance.Hide<PopupUI>();
        }
        sequence?.Kill();
    }

    private bool IsCurrentItem(int index)
    {
        return Data.DailyRewardDayIndex == index;
    }

    public void Setup()
    {
        SetUpItems();
    }

    private void SetUpItems()
    {
        var week = (Data.DailyRewardDayIndex - 1) / 7;
        if (Data.IsClaimedTodayDailyReward()) week = (Data.DailyRewardDayIndex - 2) / 7;

        for (var i = 0; i < 7; i++)
        {
            var item = DailyRewardItems[i];
            item.SetUp(this, i + 7 * week);
            if (IsCurrentItem(item.dayIndex)) CurrentItem = item;
        }

        if (CurrentItem)
        {
            if (CurrentItem.DailyRewardItemState == DailyRewardItemState.ReadyToClaim)
            {
                BtnWatchVideo.SetActive(CurrentItem.DailyRewardData.DailyRewardType == DailyRewardType.Currency);
                BtnClaim.SetActive(true);
            }
            else
            {
                BtnWatchVideo.SetActive(false);
                BtnClaim.SetActive(false);
            }
        }
        else
        {
            BtnWatchVideo.SetActive(false);
            BtnClaim.SetActive(false);
        }
           
    }

    public void OnClickBtnClaimX5Video()
    {
        AdsManager.ShowRewardAds(() =>
        {
            SoundController.Instance.PlayFX(SoundType.ClaimReward);
            //EventController.OnNotifying?.Invoke();
            CurrentItem.OnClaim(true);
        });
    }

    public void OnClickBtnClaim()
    {
        SoundController.Instance.PlayFX(SoundType.ClaimReward);
        //EventController.OnNotifying?.Invoke();
        CurrentItem.OnClaim();
    }

    public void OnClickNextDay()
    {
        Data.LastDailyRewardClaimed = DateTime.Now.AddDays(-1).ToString();
        ResetDailyReward();
        Setup();
        //EventController.OnNotifying?.Invoke();
    }
}