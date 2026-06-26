namespace WebProjectAPI.Features.Common.Helpers
{
    public static class PriceHelper
    {
        public static decimal CalculateDiscountPrice(decimal price, decimal? discountPercentage)
        {
            if (!discountPercentage.HasValue || discountPercentage <= 0)
                return price;

            decimal discountAmount = price * discountPercentage.Value / 100m;

            return Math.Round(price - discountAmount, 2);
        }
   

    }
}
