using System.Text.RegularExpressions;

#nullable enable

namespace HetsCommon
{
    public static class MinistryNameHelper
    {
        public const string CurrentName = "Ministry of Transportation and Transit";

        private static readonly Regex FormerNamePattern = new(
            @"Ministry of Transportation\s+(?:and|&?amp;|&)\s+Infrastructure",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        public static string UseCurrentName(string? value)
        {
            return string.IsNullOrEmpty(value)
                ? value ?? ""
                : FormerNamePattern.Replace(value, CurrentName);
        }
    }
}
