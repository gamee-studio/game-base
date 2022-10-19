using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardItem : MonoBehaviour
{
    [ReadOnly] public int dayIndex;
    public TextMeshProUGUI textDay;
    public TextMeshProUGUI textValue;
    public Image greenTick;
    public Image backgroundClaim;
    public Image backgroundCanNotClaim;
    public Image iconItem;
    private int coinValue;
    private DailyRewardItemState dailyRewardItemState;
    private DailyRewardData dailyRewardData;
    private PopupDailyReward popupDailyReward;
    public DailyRewardItemState DailyRewardItemState => dailyRewardItemState;

    public DailyRewardData DailyRewardData => dailyRewardData;

    public void SetUp(PopupDailyReward popup, int i)
    {
        popupDailyReward = popup;
        dayIndex = i + 1;
        SetUpData();
        SetUpUI(i);
    }

    public void SetDefaultUI()
    {
        backgroundClaim.gameObject.SetActive(false);
        backgroundCanNotClaim.gameObject.SetActive(false);
        greenTick.gameObject.SetActive(false);
    }

    private void SetUpData()
    {
        // Setup data
        dailyRewardData = Data.IsStartLoopingDailyReward
            ? ConfigController.DailyRewardConfig.DailyRewardDatasLoop[dayIndex - 1]
            : ConfigController.DailyRewardConfig.DailyRewardDatas[dayIndex - 1];

        coinValue = dailyRewardData.Value;
        // Setup states
        if (dailyRewardData.DailyRewardType == DailyRewardType.Currency)
        {
        }
        else if (dailyRewardData.DailyRewardType == DailyRewardType.Skin)
        {
            //shopItemData = ConfigController.ItemConfig.GetShopItemDataById(dailyRewardData.SkinID);
        }

        if (Data.DailyRewardDayIndex > dayIndex)
        {
            dailyRewardItemState = DailyRewardItemState.Claimed;
        }
        else if (Data.DailyRewardDayIndex == dayIndex)
        {
            if (!Data.IsClaimedTodayDailyReward())
                dailyRewardItemState = DailyRewardItemState.ReadyToClaim;
            else
                dailyRewardItemState = DailyRewardItemState.NotClaim;
        }
        else
        {
            dailyRewardItemState = DailyRewardItemState.NotClaim;
        }
    }

    public void SetUpUI(int i)
    {
        SetDefaultUI();
        textDay.text = "Day " + (i + 1);
        textValue.text = coinValue.ToString();
        switch (dailyRewardItemState)
        {
            case DailyRewardItemState.Claimed:
                backgroundClaim.gameObject.SetActive(false);
                backgroundCanNotClaim.gameObject.SetActive(true);
                greenTick.gameObject.SetActive(true);
                break;
            case DailyRewardItemState.ReadyToClaim:
                backgroundClaim.gameObject.SetActive(true);
                backgroundCanNotClaim.gameObject.SetActive(false);
                greenTick.gameObject.SetActive(false);
                break;
            case DailyRewardItemState.NotClaim:
                backgroundClaim.gameObject.SetActive(false);
                backgroundCanNotClaim.gameObject.SetActive(false);
                greenTick.gameObject.SetActive(false);
                break;
        }

        switch (dailyRewardData.DailyRewardType)
        {
            case DailyRewardType.Currency:
                textValue.gameObject.SetActive(true);
                iconItem.sprite = dailyRewardData.Icon;
                iconItem.SetNativeSize();
                break;
            case DailyRewardType.Skin:
                //Icon.sprite = shopItemData.Icon;
                iconItem.SetNativeSize();
                break;
        }
    }

    public void OnClaim(bool isClaimX5 = false)
    {
        // if (isClaimX5)
        //     FirebaseManager.OnClaimDailyRewardX5(dayIndex);
        // else
        //     FirebaseManager.OnClaimDailyReward(dayIndex);

        SoundController.Instance.PlayFX(SoundType.ClaimReward);

        // Save datas
        Data.LastDailyRewardClaimed = DateTime.Now.ToString();
        Data.DailyRewardDayIndex++;
        Data.TotalClaimDailyReward++;

        // Claim by type
        switch (dailyRewardData.DailyRewardType)
        {
            case DailyRewardType.Currency:
                Data.CurrencyTotal += coinValue * (isClaimX5 ? 5 : 1);
                break;
            case DailyRewardType.Skin:
                //shopItemData.IsUnlocked = true;
                //Data.CurrentEquippedSkin = shopItemData.Id;
                break;
        }

        //Reset Items
        popupDailyReward.Setup();
    }
}

public enum DailyRewardItemState
{
    Claimed,
    ReadyToClaim,
    NotClaim
}