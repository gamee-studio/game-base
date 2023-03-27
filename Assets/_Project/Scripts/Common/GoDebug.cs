using UnityEngine;

public class GoDebug : MonoBehaviour
{
    void Start()
    {
        Observer.DebugChanged += OnDebugChanged;
    }

    private void OnDestroy()
    {
        Observer.DebugChanged -= OnDebugChanged;
    }

    void OnDebugChanged()
    {
        gameObject.SetActive(Data.IsTesting);
    }
}
