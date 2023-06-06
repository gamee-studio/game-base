using UnityEngine;
using DG.Tweening;
using Pancake;
using UnityEngine.Serialization;

public class Popup : MonoBehaviour
{
    [SerializeField] private bool useAnimation;
    [ShowIf("useAnimation")] [SerializeField] private bool useShowAnimation;
    [ShowIf("useShowAnimation")] [SerializeField] private ShowAnimationType showAnimationType;
    [ShowIf("useAnimation")] [SerializeField] private bool useHideAnimation;
    [ShowIf("useHideAnimation")] [SerializeField] private HideAnimationType hideAnimationType;
    
    public CanvasGroup CanvasGroup => GetComponent<CanvasGroup>();
    public Canvas Canvas => GetComponent<Canvas>();
    public virtual void Show()
    {
        BeforeShow();
        gameObject.SetActive(true);
        if (useShowAnimation)
        {
            switch (showAnimationType)
            {
                case ShowAnimationType.Fade:
                    CanvasGroup.DOFade(1, ConfigController.Game.durationPopup).OnComplete(() =>
                    {
                        CanvasGroup.alpha = 0;
                        gameObject.SetActive(false);
                        AfterHidden();
                    });
                    break;
            }
        }
        else
        {
            AfterShown();
        }
    }

    public virtual void Hide()
    {
        BeforeHide();
        if (useHideAnimation)
        {
            switch (hideAnimationType)
            {
                case HideAnimationType.Fade:
                    CanvasGroup.DOFade(0, ConfigController.Game.durationPopup).OnComplete(() =>
                    {
                        CanvasGroup.alpha = 1;
                        gameObject.SetActive(false);
                        AfterHidden();
                    });
                    break;
            }
        }
        else
        {
            gameObject.SetActive(false);
            AfterHidden();
        }
    }

    protected virtual void AfterInstantiate() { }
    protected virtual void BeforeShow() { }
    protected virtual void AfterShown() { }
    protected virtual void BeforeHide() { }
    protected virtual void AfterHidden() { }
}

public enum ShowAnimationType
{
    Fade
}

public enum HideAnimationType
{
    Fade,
}

