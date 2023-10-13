using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;
using Pancake;
using UnityEngine.Serialization;

public class PopupDailyReward : Popup
{
    [SerializeField] private GameObject btnWatchVideo;
    [SerializeField] private GameObject btnClaim;

    [ReadOnly] public DailyRewardItem currentItem;
    public List<DailyRewardItem> DailyRewardItems => GetComponentsInChildren<DailyRewardItem>().ToList();

    private const string DailyOnClickClaim = "DailyOnClickClaim";
    private const string DailyOnClickClaimX5 = "DailyOnClickClaimX5";

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
            GameManager.Instance.gameState = GameState.PlayingGame;
            PopupController.Instance.Hide<PopupUI>();
        }
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
            if (IsCurrentItem(item.dayIndex)) currentItem = item;
        }

        if (currentItem)
        {
            if (currentItem.DailyRewardItemState == DailyRewardItemState.ReadyToClaim)
            {
                btnWatchVideo.SetActive(currentItem.DailyRewardData.DailyRewardType == DailyRewardType.Currency);
                btnClaim.SetActive(true);
            }
            else
            {
                btnWatchVideo.SetActive(false);
                btnClaim.SetActive(false);
            }
        }
        else
        {
            btnWatchVideo.SetActive(false);
            btnClaim.SetActive(false);
        }
           
    }

    public void OnClickBtnClaimX5Video()
    {
        AdsManager.ShowRewardAds(() =>
        {
            Observer.ClickButton?.Invoke(DailyOnClickClaimX5);
            OnClaimReward(true);
        });
    }

    public void OnClickBtnClaim()
    {
        Observer.ClickButton?.Invoke(DailyOnClickClaim);
        OnClaimReward(false);
    }

    private void OnClaimReward(bool isX5Reward)
    {
        Observer.ClaimDailyReward?.Invoke();
        
        // Save data
        Data.LastDailyRewardClaimed = DateTime.Now.ToString();
        Data.DailyRewardDayIndex++;
        Data.TotalClaimDailyReward++;
        
        currentItem.OnClaim(isX5Reward);
        
        Setup();
    }

    public void OnClickNextDay()
    {
        Data.LastDailyRewardClaimed = DateTime.Now.AddDays(-1).ToString();
        ResetDailyReward();
        Setup();
    }
}