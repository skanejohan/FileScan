namespace ScanFiles
{
    internal static class Parser
    {
        public static bool ParseString(ref string data, out string value)
        {
            return ParseBetween(ref data, "'", "'", out value);
        }

        public static bool ParseFunction0(ref string data, string functionName)
        {
            return ParseExpected(ref data, $".{functionName}()");
        }

        public static bool ParseFunction1(ref string data, string functionName, out string param)
        {
            return ParseBetween(ref data, $".{functionName}('", "')", out param);
        }

        private static bool ParseBetween(ref string data, string open, string close, out string value)
        {
            var startIndex = data.IndexOf(open);
            if (startIndex != 0)
            {
                value = "";
                return false;
            }
            var endIndex = data.IndexOf(close, open.Length);
            if (endIndex <= startIndex)
            {
                value = "";
                return false;
            }
            value = data.Substring(startIndex + open.Length, endIndex - startIndex - open.Length);
            data = data.Substring(endIndex + close.Length);
            return true;
        }

        private static bool ParseExpected(ref string data, string expectedText)
        {
            if (data.IndexOf(expectedText) == 0)
            {
                data = data.Substring(expectedText.Length);
                return true;
            }
            return false;
        }
    }
}
