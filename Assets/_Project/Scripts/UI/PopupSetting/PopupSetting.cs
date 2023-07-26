using Pancake.IAP;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class PopupSetting : Popup
{
    public void OnClickRestorePurchase()
    {
        #if UNITY_ANDROID
        
        #endif

        #if UNITY_IOS
            IAPManager.Instance.RestorePurchase();
        #endif
    }
}