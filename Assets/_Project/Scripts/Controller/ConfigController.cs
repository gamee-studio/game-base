using UnityEngine;

public class ConfigController : MonoBehaviour
{
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private SoundConfig soundConfig;
    [SerializeField] private DailyRewardConfig dailyRewardConfig;
    [SerializeField] private SkinConfig skinConfig;
    [SerializeField] private CountryConfig countryConfig;

    public static GameConfig Game;
    public static SoundConfig Sound;
    public static DailyRewardConfig DailyRewardConfig;
    public static SkinConfig SkinConfig;
    public static CountryConfig CountryConfig;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Game = gameConfig;
        Sound = soundConfig;
        DailyRewardConfig = dailyRewardConfig;
        SkinConfig = skinConfig;
        CountryConfig = countryConfig;
    }
}