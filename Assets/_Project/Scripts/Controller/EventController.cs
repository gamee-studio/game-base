using System;

public static class EventController
{
    // Data event
    public static Action OnDebugChanged;
    public static Action SaveCurrencyTotal;
    public static Action CurrencyTotalChanged;
    public static Action CurrentLevelChanged;
    public static Action OnSoundChanged;
    // Game event
    public static Action OnWinLevel;
    public static Action OnLoseLevel;
}
