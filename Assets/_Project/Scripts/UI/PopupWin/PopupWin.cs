

using DG.Tweening;
using Pancake;
using Pancake.Monetization;
using UnityEngine;

public class PopupWin : Popup
{
    public BonusArrowHandler BonusArrowHandler;
    public GameObject BtnRewardAds;
    public GameObject BtnTapToContinue;
    [ReadOnly] public int TotalMoney;

    private Sequence sequence;
    public int MoneyWin => ConfigController.Game.WinLevelMoney;
    public void SetupMoneyWin(int bonusMoney)
    {
        TotalMoney = MoneyWin + bonusMoney;
    }

    protected override void BeforeShow()
    {
        base.BeforeShow();
        SoundController.Instance.PlayFX(SoundType.ShowPopupWin);
        PopupController.Instance.Show<PopupUI>();
        Setup();
        
        sequence = DOTween.Sequence().AppendInterval(2f).AppendCallback(() => { BtnTapToContinue.SetActive(true); });
    }

    public void Setup()
    {
        BtnRewardAds.SetActive(true);
        BtnTapToContinue.SetActive(false);
    }

    protected override void BeforeHide()
    {
        base.BeforeHide();
        PopupController.Instance.Hide<PopupUI>();
    }

    public void OnClickAdsReward()
    {
        if (Data.IsTesting)
        {
            GetRewardAds();
            SoundController.Instance.PlayFX(SoundType.ClaimReward);
        }
        else
        {
            if (Advertising.IsRewardedAdReady()) BonusArrowHandler.MoveObject.StopMoving();
            AdsManager.ShowRewardAds(() =>
            {
                //FirebaseManager.OnWatchAdsRewardWin();
                GetRewardAds();
                SoundController.Instance.PlayFX(SoundType.ClaimReward);
            }, (() =>
            {
                BonusArrowHandler.MoveObject.ResumeMoving();
                BtnRewardAds.SetActive(true);
                BtnTapToContinue.SetActive(true);
            }));
        }
    }
    
    public void GetRewardAds()
    {
        Data.CurrencyTotal += TotalMoney * BonusArrowHandler.CurrentAreaItem.MultiBonus;
        BonusArrowHandler.MoveObject.StopMoving();
        BtnRewardAds.SetActive(false);
        BtnTapToContinue.SetActive(false);
        sequence?.Kill();

        DOTween.Sequence().AppendInterval(2f).AppendCallback(() => { GameManager.Instance.PlayCurrentLevel(); });
    }

    public void OnClickContinue()
    {
        Data.CurrencyTotal += TotalMoney;
        BtnRewardAds.SetActive(false);
        BtnTapToContinue.SetActive(false);

        DOTween.Sequence().AppendInterval(2f).AppendCallback(() => { GameManager.Instance.PlayCurrentLevel(); });
    }
}
