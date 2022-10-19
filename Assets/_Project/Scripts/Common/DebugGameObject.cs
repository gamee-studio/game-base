using UnityEngine;

public class DebugGameObject : MonoBehaviour
{
    void Start()
    {
        Setup();
        
        EventController.OnDebugChanged += Setup;
    }
    
    private void OnDestroy()
    {
        EventController.OnDebugChanged -= Setup;
    }

    public void Setup()
    {
        gameObject.SetActive(Data.IsTesting);
    }
}
