public class PopupHome : Popup
{
    private const string HomeOnClickStart = "HomeOnClickStart";
    private const string HomeOnClickDebug = "HomeOnClickDebug";
    private const string HomeOnClickSetting = "HomeOnClickSetting";
    private const string HomeOnClickDailyReward = "HomeOnClickDailyReward";
    private const string HomeOnClickShop = "HomeOnClickShop";
    private const string HomeOnClickTest = "HomeOnClickTest";
    
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
        Observer.ClickButton?.Invoke(HomeOnClickStart);
        GameManager.Instance.StartGame();
    }

    public void OnClickDebug()
    {
        Observer.ClickButton?.Invoke(HomeOnClickDebug);
        PopupController.Instance.Show<PopupDebug>();
    }

    public void OnClickSetting()
    {
        Observer.ClickButton?.Invoke(HomeOnClickSetting);
        PopupController.Instance.Show<PopupSetting>();
    }

    public void OnClickDailyReward()
    {
        Observer.ClickButton?.Invoke(HomeOnClickDailyReward);
        PopupController.Instance.Show<PopupDailyReward>();
    }

    public void OnClickShop()
    {
        Observer.ClickButton?.Invoke(HomeOnClickShop);
        PopupController.Instance.Show<PopupShop>();
    }
    
    public void OnClickTest()
    {
        Observer.ClickButton?.Invoke(HomeOnClickTest);
        PopupController.Instance.Show<PopupTest>();
    }
}
