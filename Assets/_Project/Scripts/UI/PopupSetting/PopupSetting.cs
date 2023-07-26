using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class PopupSetting : Popup
{
    public GameObject btnRestorePurchased;

    void Start()
    {
        #if UNITY_ANDROID
            btnRestorePurchased.SetActive(false);
        #endif
    }
    
    public void OnClickRestorePurchase()
    {
        #if UNITY_ANDROID
        
        #endif

        #if UNITY_IOS
            IAPManager.Instance.RestorePurchase();
        #endif
    }
}