namespace Pancake.Iap
{
	public static class Product
	{
		public static IAPData PurchaseRemoveads()
		{
			return IAPManager.Purchase(IAPSetting.SkusData[0]);
		}

		public static IAPData PurchaseCoin()
		{
			return IAPManager.Purchase(IAPSetting.SkusData[1]);
		}

		public static IAPData PurchaseVip()
		{
			return IAPManager.Purchase(IAPSetting.SkusData[2]);
		}

	}
}