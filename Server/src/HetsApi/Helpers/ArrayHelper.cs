namespace HetsApi.Helpers
{
    public static class ArrayHelper
    {
        public static int?[] ParseIntArray(string source)
        {
            int?[] result;

            try
            {
                string[] tokens = source.Split(',');

                result = new int?[tokens.Length];

                for (int i = 0; i < tokens.Length; i++)
                {
                    result[i] = int.Parse(tokens[i]);
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }
    }
}
