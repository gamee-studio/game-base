using Pancake.IAP;

public class IAPController : SingletonDontDestroy<IAPController>
{
    // Start is called before the first frame update
    void Start()
    {
        IAPManager.OnPurchaseSucceedEvent += OnPurchaseSucceedEvent;
    }

    private void OnPurchaseSucceedEvent(string obj)
    {
        switch (obj)
        {
            case "your_product_id":
                break;
        }
    }
}
