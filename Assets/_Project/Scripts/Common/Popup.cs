using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Popup : MonoBehaviour
{
    public GameObject Background;
    public GameObject Container;
    public bool UseShowAnimation;
    [ShowIf("UseShowAnimation")] public ShowAnimationType ShowAnimationType;
    public bool UseHideAnimation;
    [ShowIf("UseHideAnimation")] public HideAnimationType HideAnimationType;
    public CanvasGroup CanvasGroup => GetComponent<CanvasGroup>();
    public Canvas Canvas => GetComponent<Canvas>();
    public void Show()
    {
        BeforeShow();
        gameObject.SetActive(true);
        if (UseShowAnimation)
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
        if (UseHideAnimation)
        {
            switch (HideAnimationType)
            {
                case HideAnimationType.Fade:
                    CanvasGroup.DOFade(0, ConfigController.Game.DurationPopup).OnComplete(() =>
                    {
                        CanvasGroup.alpha = 1;
                        gameObject.SetActive(false);
                    });
                    break;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
        
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
    Fade,
}

