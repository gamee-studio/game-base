using Pancake;
using UnityEditor;
using UnityEngine;

public class Level : MonoBehaviour
{
    [ReadOnly] public int BonusMoney;
    
    private bool _isFingerDown;
    private bool _isFingerDrag;
    
    #if UNITY_EDITOR
    [Button]
    private void StartLevel()
    {
        Data.CurrentLevel = Utility.GetNumberInAString(gameObject.name);
        
        EditorApplication.isPlaying = true;
    }
    #endif
    
    void OnEnable()
    {
        Lean.Touch.LeanTouch.OnFingerDown += HandleFingerDown;
        Lean.Touch.LeanTouch.OnFingerUp += HandleFingerUp;
        Lean.Touch.LeanTouch.OnFingerUpdate += HandleFingerUpdate;
        
    }

    void OnDisable()
    {
        Lean.Touch.LeanTouch.OnFingerDown -= HandleFingerDown;
        Lean.Touch.LeanTouch.OnFingerUp -= HandleFingerUp;
        Lean.Touch.LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
    }
    
    void HandleFingerDown(Lean.Touch.LeanFinger finger)
    {
        Debug.Log("OnFingerDown");
        if (!finger.IsOverGui)
        {
            _isFingerDown = true;
        }
        
        // var ray = finger.GetRay(Camera);
        // var hit = default(RaycastHit);
        //
        // if (Physics.Raycast(ray, out hit, float.PositiveInfinity)) { //ADDED LAYER SELECTION
        //     Debug.Log(hit.collider.gameObject);
        // }
    }
    
    void HandleFingerUp(Lean.Touch.LeanFinger finger)
    {
        Debug.Log("OnFingerUp");
        _isFingerDown = false;
    }
    
    void HandleFingerUpdate(Lean.Touch.LeanFinger finger)
    {
        if (_isFingerDown)
        {
            _isFingerDrag = true;
            Debug.Log("OnFingerDrag");
        }
    }
    
    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    public void OnWinGame()
    {
        
    }

    public void OnLoseGame()
    {
        
    }
    
}
