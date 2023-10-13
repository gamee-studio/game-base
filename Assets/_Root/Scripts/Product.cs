namespace Pancake.IAP
{
	public static class Product
	{
		public static IAPData Purchase50000Coin()
		{
			return IAPManager.Purchase(IAPSettings.SkusData[0]);
		}

		public static bool IsPurchased50000Coin()
		{
			return IAPManager.Instance.IsPurchased(IAPSettings.SkusData[0].sku.Id);
		}

		public static IAPData PurchaseRemoveads()
		{
			return IAPManager.Purchase(IAPSettings.SkusData[1]);
		}

		public static bool IsPurchasedRemoveads()
		{
			return IAPManager.Instance.IsPurchased(IAPSettings.SkusData[1].sku.Id);
		}

	}
}