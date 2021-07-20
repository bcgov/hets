namespace HetsBceid
{
    public static class StringExtensions
    {

        public static bool IsNotEmpty(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsIdirUser(this string str)
        {
            return str.ToUpperInvariant() == "INTERNAL";
        }

        public static bool IsBusinessUser(this string str)
        {
            return str.ToUpperInvariant() == "BUSINESS";
        }
    }
}
