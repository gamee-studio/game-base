using System;

public static class Observer
{
    #region DataSystem
    // Debug
    public static Action DebugChanged;
    // Currency
    public static Action SaveCurrencyTotal;
    public static Action CurrencyTotalChanged;
    // Level Spawn
    public static Action CurrentLevelChanged;
    // Setting
    public static Action MusicChanged;
    public static Action SoundChanged;
    public static Action VibrationChanged;
    #endregion


    // Game event
    public static Action OnWinLevel;
    public static Action OnLoseLevel;
}
