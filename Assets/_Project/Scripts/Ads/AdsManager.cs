using System;
using Pancake.Threading.Tasks;
using Pancake.Monetization;

public class AdsManager
{
    public static int TotalLevelWinLose;
    public static float TotalTimesPlay;
    public static bool IsInitialized => Advertising.IsInitialized;

    private static Action _callbackInterstitialCompleted;
    private static Action _callbackRewardCompleted;
    private static Action _callBackRewardSkipped;

    public static async void Initialize()
    {
        Advertising.InterstitialAdCompletedEvent += OnInterstitialAdCompleted;
        Advertising.RewardedAdCompletedEvent += OnRewardAdCompleted;
        Advertising.RewardedAdSkippedEvent += OnRewardedAdSkippedEvent;
        await UniTask.WaitUntil(() => Advertising.ApplovinAdClient.IsInitialized);
        Advertising.ApplovinAdClient.OnRewardedAdRevenuePaid += OnRevenuePaid;
        Advertising.ApplovinAdClient.OnBannerAdRevenuePaid += OnRevenuePaid;
        Advertising.ApplovinAdClient.OnInterstitialAdRevenuePaid += OnRevenuePaid;
    }
    
    private static void OnRevenuePaid(MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
        // send ad revenue info to Adjust
        // AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
        // adRevenue.setRevenue(adInfo.Revenue, "USD");
        // adRevenue.setAdRevenueNetwork(adInfo.NetworkName);
        // adRevenue.setAdRevenuePlacement(adInfo.Placement);
        // adRevenue.setAdRevenueUnit(adInfo.AdUnitIdentifier);
        // Adjust.trackAdRevenue(adRevenue);
    }

    private static void OnInterstitialAdCompleted(EInterstitialAdNetwork val)
    {
        _callbackInterstitialCompleted?.Invoke();
        _callbackInterstitialCompleted = null;
    }
    
    private static void OnRewardAdCompleted(ERewardedAdNetwork val)
    {
        _callbackRewardCompleted?.Invoke();
        _callbackRewardCompleted = null;
    }
    
    private static void OnRewardedAdSkippedEvent(ERewardedAdNetwork val)
    {
        _callBackRewardSkipped?.Invoke();
    }
    
    public static bool IsSufficientConditionToShowInter()
    {
        if (Data.CurrentLevel>Data.LevelTurnOnInterstitial && TotalLevelWinLose >= Data.CounterNumbBetweenTwoInterstitial && TotalTimesPlay>=Data.TimeWinBetweenTwoInterstitial)
        {
            return true;
        }

        return false;
    }

    public static void ShowInterstitial(Action callBack)
    {
        if (IsSufficientConditionToShowInter())
        {
            FirebaseManager.OnRequestInterstitial();
            if (Advertising.IsInterstitialAdReady())
            {
                FirebaseManager.OnShowInterstitial();
                
                _callbackInterstitialCompleted = callBack;
                Advertising.ShowInterstitialAd();
                Reset();
            }
            else
            {
                callBack?.Invoke();
            }
        }
        else
        {
            callBack?.Invoke();
        }
    }
    
    public static void ShowRewardAds(Action callBack, Action skip = null)
    {
        if (Data.IsTesting)
        {
            _callbackRewardCompleted = callBack;
            _callbackRewardCompleted?.Invoke();
        }
        else
        {
            FirebaseManager.OnRequestReward();
            if (Advertising.IsRewardedAdReady())
            {
                FirebaseManager.OnShowReward();
            
                _callbackRewardCompleted = callBack;
                Advertising.ShowRewardedAd();
            }
        }
    }

    private static void Reset()
    {
        TotalTimesPlay = 0;
        TotalLevelWinLose = 0;
    }

    public static void ShowBanner()
    {
        FirebaseManager.OnShowBanner();
        
        Advertising.ShowBannerAd();
    }
    
    public static void HideBanner()
    {
        Advertising.HideBannerAd();
    }
}
