

public class PopupWin : Popup
{
    // public int MoneyWin => ConfigController.Game.c;
    // public void SetupMoneyWin(int money)
    // {
    //     TotalMoney = 
    // }
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
