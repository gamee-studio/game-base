using System;
using System.Linq;
using MoreMountains.NiceVibrations;
using Pancake;
using Pancake.Monetization;
using Random = System.Random;

public class PopupTest : Popup
{
    
    protected override void BeforeShow()
    {
        base.BeforeShow();
        AdsManager.ShowBanner();
    }
    
    protected override void BeforeHide()
    {
        base.BeforeHide();
        AdsManager.HideBanner();
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

}
