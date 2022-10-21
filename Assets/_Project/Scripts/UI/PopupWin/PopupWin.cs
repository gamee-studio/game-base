

public class PopupWin : Popup
{
    [ReadOnly] public int TotalMoney; 
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
    }

    protected override void BeforeHide()
    {
        base.BeforeHide();
        PopupController.Instance.Hide<PopupUI>();
    }

    public void OnClickContinue()
    {
        GameManager.Instance.PrepareLevel();
        GameManager.Instance.StartGame();
    }
}
