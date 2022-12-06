using System;
using Pancake;
using UnityEditor;
using UnityEngine;

public class Level : MonoBehaviour
{
    [ReadOnly] public int BonusMoney;

    [Button]
    private void StartLevel()
    {
        Data.CurrentLevel = Utility.GetNumberInAString(gameObject.name);
        
        EditorApplication.isPlaying = true;
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
