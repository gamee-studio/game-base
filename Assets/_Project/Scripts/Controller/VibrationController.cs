using Lofelt.NiceVibrations;

public class VibrationController : SingletonDontDestroy<VibrationController>
{
    void Start()
    {
        Setup();

        Observer.VibrationChanged += Setup;
    }

    void Setup()
    {
        HapticController.hapticsEnabled = Data.VibrateState;
    }

    public void HapticLight()
    {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
    
    public void HapticMedium()
    {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
    }
    
    public void HapticHeavy()
    {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
    }
}
