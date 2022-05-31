using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCustom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button.ButtonClickedEvent onClick;
    [SerializeField] private Button.ButtonClickedEvent onPress;

    public bool CanClick = true;
    public bool HavePressEffect = true;
    private bool isMoveEnter = false;
    private Vector3 localScale;
    
    private void Awake()
    {
        localScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanClick)
        {
            onPress?.Invoke();
            if (HavePressEffect) transform.DOScale(localScale-(Vector3.one*0.1f), .01f).SetEase(Ease.OutQuint);
            isMoveEnter = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CanClick)
        {
            transform.localScale = localScale;
            if (isMoveEnter)
            {
                onClick.Invoke();
                SoundController.Instance.PlayFX(SoundType.ButtonClick);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMoveEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMoveEnter = false;
    }
}