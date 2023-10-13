using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class PopupSetting : Popup
{
    public GameObject btnRestorePurchased;
    
    private const string SettingOnClickRestorePurchase = "OnClickRestorePurchase";

    void Start()
    {
        #if UNITY_ANDROID
            btnRestorePurchased.SetActive(false);
        #endif
    }
    
    public void OnClickRestorePurchase()
    {
        Observer.ClickButton?.Invoke(SettingOnClickRestorePurchase);
        #if UNITY_IOS
            IAPManager.Instance.RestorePurchase();
        #endif
    }
}