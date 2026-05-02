using System.Text.RegularExpressions;

namespace WebProjectAPI.Features.Common.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string text)
        {
            text = text.ToLower().Trim();

            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

            text = Regex.Replace(text, @"\s+", "-");

            return text;
        }
    }
}