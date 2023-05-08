using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class VibrationController : SingletonDontDestroy<VibrationController>
{
    void Start()
    {
        Setup();

        Observer.VibrationChanged += Setup;
    }

    void Setup()
    {
        MMVibrationManager.SetHapticsActive(Data.VibrateState);
    }

    public void HapticLight()
    {
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }
    
    public void HapticMedium()
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }
    
    public void HapticHeavy()
    {
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
    }
}
