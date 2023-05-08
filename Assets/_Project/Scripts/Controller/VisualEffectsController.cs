using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class VisualEffectsController : SingletonDontDestroy<VisualEffectsController>
{
    public List<VisualEffectData> visualEffectDatas;

    public VisualEffectData GetVisualEffectDataByType(string name)
    {
        return visualEffectDatas.Find(item => item.name == name);
    }

    public void SpawnEffect(string name,Vector3 position, Transform parent, Vector3? localScale = null, bool isDestroyedOnEnd = true, float timeDestroy = 3f)
    {
        VisualEffectData visualEffectData = GetVisualEffectDataByType(name);
        if (visualEffectData != null)
        {
            GameObject randomEffect = visualEffectData.GetRandomEffect();
            GameObject effect = Instantiate(randomEffect, parent, false);
            effect.transform.position = position;
            if (localScale != null) effect.transform.localScale = localScale.Value;
            if (isDestroyedOnEnd) Destroy(effect, timeDestroy);
        }
    }
}

[Serializable]
public class VisualEffectData
{
    public string name;
    public List<GameObject> effects;
    

    public GameObject GetRandomEffect()
    {
        return effects[Random.Range(0, effects.Count)];
    }
}