using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaItem : MonoBehaviour
{
    public int MultiBonus = 1;
    public GameObject BoderLight;

    public void ActivateBoderLight()
    {
        BoderLight.SetActive(true);
    }

    public void DeActivateBoderLight()
    {
        BoderLight.SetActive(false);
    }
}