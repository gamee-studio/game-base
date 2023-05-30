using System;
using System.Reflection;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Pancake;
using UnityEngine;

public class FirebaseController : SingletonDontDestroy<FirebaseController>
{
    [ReadOnly] public DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    [ReadOnly] public bool isInitialized;
    protected override void Awake()
    {
        Initialize();
    }

    void Start()
    {
        Observer.StartLevel += OnStartLevel;
        Observer.WinLevel += OnWinLevel;
        Observer.LoseLevel += OnLoseLevel;
        
        Observer.RequestBanner += OnRequestBanner;
        Observer.ShowBanner += OnShowBanner;
        Observer.RequestInterstitial += OnRequestInterstitial;
        Observer.ShowInterstitial += OnShowInterstitial;
        Observer.RequestReward += OnRequestReward;
        Observer.ShowReward += OnShowReward;
    }

    #region FirebaseInitGetRemoteConfig

    private void Initialize()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();

                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private async void InitializeFirebase()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        var defaults = new System.Collections.Generic.Dictionary<string, object>
        {
            {Constant.UseLevelAbTesting, Data.DEFAULT_USE_LEVEL_AB_TESTING},
            {Constant.LevelTurnONInterstitial, Data.DEFAULT_LEVEL_TURN_ON_INTERSTITIAL},
            {Constant.CounterNumberBetweenTwoInterstitial, Data.DEFAULT_COUNTER_NUMBER_BETWEEN_TWO_INTERSTITIAL},
            {Constant.SpaceTimeWinBetweenTwoInterstitial, Data.DEFAULT_SPACE_TIME_WIN_BETWEEN_TWO_INTERSTITIAL},
            {Constant.ShowInterstitialONLoseGame, Data.DEFAULT_SHOW_INTERSTITIAL_ON_LOSE_GAME},
            {Constant.SpaceTimeLoseBetweenTwoInterstitial, Data.DEFAULT_SPACE_TIME_LOSE_BETWEEN_TWO_INTERSTITIAL},
        };
        await Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task =>
            {
                // [END set_defaults]
                Debug.Log("RemoteConfig configured and ready!");
            });

        await FetchDataAsync();
    }

    private Task FetchDataAsync()
    {
        Debug.Log("<color=Green>Fetching data from Firebase ...</color>");
        System.Threading.Tasks.Task fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        return fetchTask.ContinueWithOnMainThread(tast =>
        {
            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
            //SET NEW DATA FROM REMOTE CONFIG
            if (info.LastFetchStatus == LastFetchStatus.Success)
            {
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task =>
                    {
                        Debug.LogWarning($"Remote data loaded and ready (last fetch time {info.FetchTime}).");
                    });

                Data.UseLevelABTesting = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.UseLevelAbTesting).StringValue);
                Data.LevelTurnOnInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.LevelTurnONInterstitial).StringValue);
                Data.CounterNumbBetweenTwoInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.CounterNumberBetweenTwoInterstitial).StringValue);
                Data.TimeWinBetweenTwoInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.SpaceTimeWinBetweenTwoInterstitial).StringValue);
                Data.UseShowInterstitialOnLoseGame = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.ShowInterstitialONLoseGame).StringValue);
                Data.TimeLoseBetweenTwoInterstitial = int.Parse(FirebaseRemoteConfig.DefaultInstance
                    .GetValue(Constant.SpaceTimeLoseBetweenTwoInterstitial).StringValue);

                Debug.LogWarning("<color=Green>Firebase Remote Config Fetching Values</color>");
                Debug.LogWarning($"<color=Green>Data.UseLevelABTesting: {Data.UseLevelABTesting}</color>");
                Debug.LogWarning($"<color=Green>Data.LevelTurnOnInterstitial: {Data.LevelTurnOnInterstitial}</color>");
                Debug.LogWarning(
                    $"<color=Green>Data.CounterNumbBetweenTwoInterstitial: {Data.CounterNumbBetweenTwoInterstitial}</color>");
                Debug.LogWarning(
                    $"<color=Green>Data.TimeWinBetweenTwoInterstitial: {Data.TimeWinBetweenTwoInterstitial}</color>");
                Debug.LogWarning(
                    $"<color=Green>Data.UseShowInterstitialOnLoseGame: {Data.UseShowInterstitialOnLoseGame}</color>");
                Debug.LogWarning(
                    $"<color=Green>Data.TimeLoseBetweenTwoInterstitial: {Data.TimeLoseBetweenTwoInterstitial}</color>");
                Debug.Log("<color=Green>Firebase Remote Config Fetching completed!</color>");
            }
            else
            {
                Debug.Log("<color=Red>Fetching data did not completed!</color>");
            }

            isInitialized = true;
        });
    }

    #endregion

    #region TrackingGameplay

    private void OnStartLevel(Level level)
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Parameter[] parameters =
        {
            new Parameter("level_name", level.gameObject.name),
        };
        LogEvent(function.Name, parameters);
    }

    private void OnLoseLevel(Level level)
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Parameter[] parameters =
        {
            new Parameter("level_name", level.gameObject.name),
        };
        LogEvent(function.Name, parameters);
    }

    private void OnWinLevel(Level level)
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Parameter[] parameters =
        {
            new Parameter("level_name", level.gameObject.name),
        };
        LogEvent(function.Name, parameters);
    }

    private void OnReplayLevel(Level level)
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        Parameter[] parameters =
        {
            new Parameter("level_name", level.gameObject.name),
        };
        LogEvent(function.Name, parameters);
    }

    #endregion

    #region TrackingGameSystem

    private void OnClickButton(string buttonName)
    {
        LogEvent(buttonName);
    }

    public static void OnRequestInterstitial()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    public static void OnShowInterstitial()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    public static void OnRequestReward()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    public static void OnShowReward()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    public static void OnRequestBanner()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    public static void OnShowBanner()
    {
        MethodBase function = MethodBase.GetCurrentMethod();
        LogEvent(function.Name);
    }

    #endregion

    #region BaseLogFunction

    public static bool IsMobile()
    {
        return (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer);
    }

    public static void LogEvent(string paramName, Parameter[] parameters)
    {
        if (!IsMobile()) return;
        try
        {
            FirebaseAnalytics.LogEvent(paramName, parameters);
        }
        catch (Exception e)
        {
            Debug.LogError("Event log error: " + e.ToString());
            throw;
        }
    }

    public static void LogEvent(string paramName)
    {
        if (!IsMobile()) return;
        try
        {
            FirebaseAnalytics.LogEvent(paramName);
        }
        catch (Exception e)
        {
            Debug.LogError("Event log error: " + e.ToString());
            throw;
        }
    }

    #endregion
}