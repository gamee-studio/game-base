using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IAPConfig", menuName = "ScriptableObject/IAPConfig")]
public class IAPConfig : ScriptableObject
{
    public List<IAPData> IAPDatas;

    public void ResetAllIAP()
    {
        foreach (IAPData item in IAPDatas)
        {
            if (item.ConsumableState == ConsumableState.NonConsumable)
            {
                item.IsUnlocked = false;
            }
        }
    }
}

[Serializable]
public class IAPData
{
    public string Name;
    public IAPRewardType IAPRewardType;
    public int ValueReward;
    public ConsumableState ConsumableState;
    public Sprite Icon;
    public Vector2 PrefixSize;
    
    public bool IsUnlocked
    {
        get
        {
            Data.IdIAPCheckUnlocked = Name;
            return Data.IsSkinUnlocked;
        }

        set
        {
            Data.IdIAPCheckUnlocked = Name;
            Data.IsIAPUnlocked = value;
        }
    }
}

