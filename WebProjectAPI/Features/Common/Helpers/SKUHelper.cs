namespace WebProjectAPI.Features.Common.Helpers
{
    public static class SKUHelper
    {
        public static string GenerateSKU(string prefix)
        {
            return $"{prefix.ToUpper()}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
}