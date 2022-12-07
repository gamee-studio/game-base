using Pancake;
using UnityEngine;

public class BonusArrowHandler : MonoBehaviour
{
    [ReadOnly] public AreaItem CurrentAreaItem;
    public MoveObject MoveObject => GetComponent<MoveObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BonusArea"))
        {
            CurrentAreaItem = other.GetComponent<AreaItem>();
            CurrentAreaItem.ActivateBorderLight();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BonusArea"))
        {
            other.GetComponent<AreaItem>().DeActivateBorderLight();
        }
    }
}