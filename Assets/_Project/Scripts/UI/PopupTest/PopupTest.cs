using Pancake.Monetization;

public class PopupTest : Popup
{
    private bool _isShowBanner;
    
    public void OnClickInAppReview()
    {
        InAppReviewController.Instance.Review();
    }

    public void OnClickBtnShowBannerAds()
    {
        if (!_isShowBanner)
        {
            Advertising.ShowBannerAd();
        }
        else
        {
            Advertising.HideBannerAd();
        }
    }

    public void OnClickBtnShowInterstitialAds()
    {
        Advertising.ShowInterstitialAd();
    }
    
    public void OnClickBtnShowRewardAds()
    {
        Advertising.ShowRewardedAd();
    }
    
    public void OnClickLightVibration()
    {
        VibrationController.Instance.HapticLight();
    }
    
    public void OnClickMediumVibration()
    {
        VibrationController.Instance.HapticMedium();
    }
    
    public void OnClickHeavyVibration()
    {
        VibrationController.Instance.HapticHeavy();
    }

    public void OnClickBuyCoin()
    {
        Data.MoneyTotal += 10000;
    }

    public void OnClickRemoveAds()
    {
        
    }
}
