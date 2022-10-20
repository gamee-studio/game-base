using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Popup : MonoBehaviour
{
    public GameObject Background;
    public GameObject Container;
    public bool UseAnimation;
    [ShowIf("UseAnimation")] public ShowAnimationType ShowAnimationType;
    [ShowIf("UseAnimation")] public HideAnimationType HideAnimationType;
    public CanvasGroup CanvasGroup => GetComponent<CanvasGroup>();
    public Canvas Canvas => GetComponent<Canvas>();
    public void Show()
    {
        BeforeShow();
        gameObject.SetActive(true);
        if (UseAnimation)
        {
            switch (ShowAnimationType)
            {
                case ShowAnimationType.OutBack:
                    DOTween.Sequence().OnStart(() => Container.transform.localScale = Vector3.one*.5f)
                        .Append(Container.transform.DOScale(Vector3.one, ConfigController.Game.DurationPopup).SetEase(Ease.OutBack).OnComplete(() =>
                        {
                            AfterShown();
                        }));
                    break;
            }
        }
        else
        {
            AfterShown();
        }
        
        
    }

    public void Hide()
    {
        BeforeHide();
        gameObject.SetActive(false);
        AfterHidden();
    }

    protected virtual void AfterInstantiate() { }
    protected virtual void BeforeShow() { }
    protected virtual void AfterShown() { }
    protected virtual void BeforeHide() { }
    protected virtual void AfterHidden() { }
}

public enum ShowAnimationType
{
    OutBack,
}

public enum HideAnimationType
{
    FadeCenter,
    MoveToLeft,
    MoveToRight,
    MoveToDown,
    MoveToUp,
}

