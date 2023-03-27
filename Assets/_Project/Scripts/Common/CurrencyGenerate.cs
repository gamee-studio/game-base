using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CurrencyGenerate : MonoBehaviour
{
    public GameObject overlay;
    public GameObject coinPrefab;
    public GameObject from;
    public GameObject to;
    public int numberCoin;
    public int delay;
    public float durationNear;
    public float durationTarget;
    public Ease easeNear;
    public Ease easeTarget;
    public float scale = 1;
    private int _numberCoinMoveDone;
    private Action _moveOneCoinDone;
    private Action _moveAllCoinDone;

    public void SetFromGameObject(GameObject from)
    {
        this.from = from;
    }

    public void SetToGameObject(GameObject to)
    {
        this.to = to;
    }

    private void Start()
    {
        overlay.SetActive(false);
    }

    public async void GenerateCoin(Action moveOneCoinDone, Action moveAllCoinDone, GameObject from = null, GameObject to = null, int numberCoin = -1)
    {
        this._moveOneCoinDone = moveOneCoinDone;
        this._moveAllCoinDone = moveAllCoinDone;
        this.from = from == null ? this.from : from;
        this.to = to == null ? this.to : to;
        this.numberCoin = numberCoin < 0 ? this.numberCoin : numberCoin;
        _numberCoinMoveDone = 0;
        overlay.SetActive(true);
        for (int i = 0; i < this.numberCoin; i++)
        {
            await Task.Delay(Random.Range(0, delay));
            GameObject coin = Instantiate(coinPrefab, transform);
            coin.transform.localScale = Vector3.one * scale;
            coin.transform.position = this.from.transform.position;
            MoveCoin(coin);
        }
    }

    private void MoveCoin(GameObject coin)
    {
        //Observer.PlayOnce(SoundType.CoinMove);
        MoveToNear(coin).OnComplete(() =>
        {
            MoveToTarget(coin).OnComplete(() =>
            {
                _numberCoinMoveDone++;
                Destroy(coin);
                _moveOneCoinDone?.Invoke();
                if (_numberCoinMoveDone >= numberCoin)
                {
                    _moveAllCoinDone?.Invoke();
                    overlay.SetActive(false);
                }
            });
        });
    }

    private DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> MoveTo(Vector3 endValue, GameObject coin, float duration, Ease ease)
    {
        return coin.transform.DOMove(endValue, duration).SetEase(ease);
    }

    private DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> MoveToNear(GameObject coin)
    {
        return MoveTo(coin.transform.position + (Vector3)Random.insideUnitCircle*3, coin, durationNear, easeNear);
    }

    private DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> MoveToTarget(GameObject coin)
    {
        return MoveTo(to.transform.position, coin, durationTarget, easeTarget);
    }
    
    public void SetNumberCoin(int coin)
    {
        numberCoin = coin;
    }
}
