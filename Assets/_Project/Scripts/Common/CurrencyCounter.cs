using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CurrencyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyAmountText;
    [SerializeField] private int stepCount = 10;
    [SerializeField] private float delayTime = .01f;
    [SerializeField] private CurrencyGenerate currencyGenerate;

    private int _currentCoin;

    private void Start()
    {
        Observer.SaveCurrencyTotal += SaveCurrency;
        Observer.CurrencyTotalChanged += UpdateCurrencyAmountText;
        currencyAmountText.text = Data.CurrencyTotal.ToString();
    }

    private void SaveCurrency()
    {
        _currentCoin = Data.CurrencyTotal;
    }
    
    private void UpdateCurrencyAmountText()
    {
        if ( Data.CurrencyTotal > _currentCoin)
        {
            IncreaseCurrency();
        }
        else
        {
            DecreaseCurrency();
        }
    }

    private void IncreaseCurrency()
    {
        bool isPopupUIActive = PopupController.Instance.Get<PopupUI>().isActiveAndEnabled;
        if (!isPopupUIActive) PopupController.Instance.Show<PopupUI>();
        bool isFirstMove = false;
        currencyGenerate.GenerateCoin(() =>
        {
            if (!isFirstMove)
            {
                isFirstMove = true;
                int currentCurrencyAmount = int.Parse(currencyAmountText.text);
                int nextAmount = (Data.CurrencyTotal - currentCurrencyAmount)/stepCount;
                int step = stepCount;
                CurrencyTextCount(currentCurrencyAmount, nextAmount,step);
            }
        }, ()=>
        {
            Observer.CoinMove?.Invoke();
            if (!isPopupUIActive) PopupController.Instance.Hide<PopupUI>();
        });
    }

    private void DecreaseCurrency()
    {
        int currentCurrencyAmount = int.Parse(currencyAmountText.text);
        int nextAmount = (Data.CurrencyTotal - currentCurrencyAmount)/stepCount;
        int step = stepCount;
        CurrencyTextCount(currentCurrencyAmount, nextAmount,step);
    }

    private void CurrencyTextCount(int currentCurrencyValue,int nextAmountValue,int stepCount)
    {
        if (stepCount == 0)
        {
            currencyAmountText.text = Data.CurrencyTotal.ToString();
            return;
        }
        int totalValue = (currentCurrencyValue + nextAmountValue);
        DOTween.Sequence().AppendInterval(delayTime).SetUpdate(isIndependentUpdate:true).AppendCallback(() =>
        {
            currencyAmountText.text = totalValue.ToString();
        }).AppendCallback(()=>
        {
            CurrencyTextCount(totalValue, nextAmountValue, stepCount - 1);
        });
    }
}