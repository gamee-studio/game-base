using DG.Tweening;
using Pancake;
using Pancake.Monetization;
using UnityEngine;

public class PopupWin : Popup
{
    [SerializeField] private BonusArrowHandler bonusArrowHandler;
    [SerializeField] private GameObject btnRewardAds;
    [SerializeField] private GameObject btnTapToContinue; 
    [SerializeField] [ReadOnly] private int totalMoney;

    private Sequence sequence;
    public int MoneyWin => ConfigController.Game.winLevelMoney;
    public void SetupMoneyWin(int bonusMoney)
    {
        totalMoney = MoneyWin + bonusMoney;
    }

    protected override void BeforeShow()
    {
        base.BeforeShow();
        PopupController.Instance.Show<PopupUI>();
        Setup();
        
        sequence = DOTween.Sequence().AppendInterval(2f).AppendCallback(() => { btnTapToContinue.SetActive(true); });
    }

    public void Setup()
    {
        btnRewardAds.SetActive(true);
        btnTapToContinue.SetActive(false);
        bonusArrowHandler.MoveObject.ResumeMoving();
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
            Observer.ClaimReward?.Invoke();
        }
        else
        {
            AdsManager.ShowRewardAds(() =>
            {
                GetRewardAds();
                Observer.ClaimReward?.Invoke();
            }, skipCallback: () =>
            {
                bonusArrowHandler.MoveObject.ResumeMoving();
                btnRewardAds.SetActive(true);
                btnTapToContinue.SetActive(true);
            },closeCallback: () =>
            {
                bonusArrowHandler.MoveObject.ResumeMoving();
                btnRewardAds.SetActive(true);
                btnTapToContinue.SetActive(true);
            });
        }
    }
    
    public void GetRewardAds()
    {
        Data.CurrencyTotal += totalMoney * bonusArrowHandler.CurrentAreaItem.MultiBonus;
        bonusArrowHandler.MoveObject.StopMoving();
        btnRewardAds.SetActive(false);
        btnTapToContinue.SetActive(false);
        sequence?.Kill();

        DOTween.Sequence().AppendInterval(2f).AppendCallback(() => { GameManager.Instance.PlayCurrentLevel(); });
    }

    public void OnClickContinue()
    {
        Data.CurrencyTotal += totalMoney;
        btnRewardAds.SetActive(false);
        btnTapToContinue.SetActive(false);

        DOTween.Sequence().AppendInterval(2f).AppendCallback(() => { GameManager.Instance.PlayCurrentLevel(); });
    }
}
