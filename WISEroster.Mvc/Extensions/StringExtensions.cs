namespace WISEroster.Mvc.Extensions
{
    public static class StringExtensions
    {
        public static string IdToName(this string val)
        {
            return val.StartsWith("p:")? val.Substring(2):val;
        }
        public static string NameToId(this string val)
        {
            return "p:" + val;
        }
    }
}