using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinConfig", menuName = "ScriptableObject/SkinConfig")]
public class SkinConfig : ScriptableObject
{
    public List<SkinData> SkinDatas;

    public void ResetAllSkins()
    {
        foreach (SkinData item in SkinDatas)
        {
            item.IsUnlocked = false;
        }
    }

    public void UnlockAllSkins()
    {
        SkinDatas.ForEach(item => item.IsUnlocked = true);
    }

    public SkinData GetDailySkin()
    {
        return SkinDatas.Find(item => item.BuyType == BuyType.DailyReward && !item.IsUnlocked);
    }

    public SkinData GetUnlockGiftSkin()
    {
        return SkinDatas.Find(item => item.BuyType == BuyType.Money && !item.IsUnlocked);
    }
}

[Serializable]
public class SkinData
{
    [GUID] public string Id;
    public BuyType BuyType;
    public Sprite Icon;
    public Vector2 PrefixSizeIcon;
    public GameObject Prefab;
    public int Price;

    public bool IsUnlocked
    {
        get
        {
            Data.IdSkinCheckUnlocked = Id;
            return Data.IsSkinUnlocked;
        }

        set
        {
            Data.IdSkinCheckUnlocked = Id;
            if (BuyType == BuyType.Default)
            {
                Data.IsSkinUnlocked = true;
            }
            else
            {
                Data.IsSkinUnlocked = value;
            }
        }
    }
}