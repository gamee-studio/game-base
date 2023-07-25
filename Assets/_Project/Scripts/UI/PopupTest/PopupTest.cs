using MoreMountains.NiceVibrations;
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
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }
    
    public void OnClickMediumVibration()
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }
    
    public void OnClickHeavyVibration()
    {
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
    }

    public void OnClickBuyCoin()
    {
        
    }

    public void OnClickRemoveAds()
    {
        
    }
}
