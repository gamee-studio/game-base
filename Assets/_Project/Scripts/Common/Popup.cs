using UnityEngine;
using DG.Tweening;
using Pancake;
using UnityEngine.Serialization;

public class Popup : MonoBehaviour
{
    [SerializeField] private bool useAnimation;
    [ShowIf("useAnimation")] [SerializeField] private GameObject background;
    [ShowIf("useAnimation")] [SerializeField] private GameObject container;
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
                case ShowAnimationType.OutBack:
                    DOTween.Sequence().OnStart(() => container.transform.localScale = Vector3.one*.9f)
                        .Append(container.transform.DOScale(Vector3.one, ConfigController.Game.durationPopup).SetEase(Ease.OutBack).OnComplete(AfterShown));
                    break;
                case ShowAnimationType.Flip:
                    DOTween.Sequence().OnStart(() => container.transform.localEulerAngles = new Vector3(0,180,0))
                        .Append(container.transform.DORotate(Vector3.zero, ConfigController.Game.durationPopup)).SetEase(Ease.Linear).OnComplete(AfterShown);
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
                case HideAnimationType.InBack:
                    DOTween.Sequence().Append(container.transform.DOScale(Vector3.one*.7f, ConfigController.Game.durationPopup).SetEase(Ease.InBack).OnComplete(() =>
                        {
                            gameObject.SetActive(false);
                            AfterShown();
                        }));
                    break;
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
    OutBack,
    Flip,
}

public enum HideAnimationType
{
    InBack,
    Fade,
}

