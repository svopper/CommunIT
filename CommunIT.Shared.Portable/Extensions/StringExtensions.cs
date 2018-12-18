namespace CommunIT.Shared.Portable.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpaceOrEmpty(this string s) => string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);
    }
}