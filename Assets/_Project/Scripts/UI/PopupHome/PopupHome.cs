using System.Reflection;
using Pancake;

public class PopupHome : Popup
{
    protected override void BeforeShow()
    {
        base.BeforeShow();
        PopupController.Instance.Show<PopupUI>();
    }
    
    protected override void BeforeHide()
    {
        base.BeforeHide();
        PopupController.Instance.Hide<PopupUI>();
    }

    public void OnClickStart()
    {
        GameManager.Instance.StartGame();
    }

    public void OnClickDebug()
    {
        PopupController.Instance.Show<PopupDebug>();
    }

    public void OnClickSetting()
    {
        PopupController.Instance.Show<PopupSetting>();
    }

    public void OnClickDailyReward()
    {
        PopupController.Instance.Show<PopupDailyReward>();
    }

    public void OnClickShop()
    {
        PopupController.Instance.Show<PopupShop>();
    }
    
    public void OnClickTest()
    {
        PopupController.Instance.Show<PopupTest>();
    }
}
