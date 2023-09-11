using System;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using Random = UnityEngine.Random;


public class VisualEffectsController : SingletonDontDestroy<VisualEffectsController>
{
    public List<VisualEffectData> visualEffectDatas;

    public VisualEffectData GetVisualEffectDataByType(EffectName effectName)
    {
        return visualEffectDatas.Find(item => item.name == effectName);
    }

    public void SpawnEffect(EffectName name,Vector3 position, Transform parent, Vector3? localScale = null, bool isDestroyedOnEnd = true, float timeDestroy = 3f)
    {
        VisualEffectData visualEffectData = GetVisualEffectDataByType(name);
        if (visualEffectData != null)
        {
            GameObject randomEffect = visualEffectData.GetRandomEffect();
            GameObject effect = LeanPool.Spawn(randomEffect, parent, false);
            effect.transform.position = position;
            if (localScale != null) effect.transform.localScale = localScale.Value;
            if (isDestroyedOnEnd) LeanPool.Despawn(effect, timeDestroy);
        }
    }
}

[Serializable]
public class VisualEffectData
{
    public EffectName name;
    public List<GameObject> effects;
    

    public GameObject GetRandomEffect()
    {
        return effects[Random.Range(0, effects.Count)];
    }
}

public enum EffectName
{
    Default,
}