namespace WingLoader_Generics.PrimitiveHelpers
{
    /// <summary>
    /// Extension methods for clean string conversion from legacy fixed-length buffers.
    /// </summary>
    internal static class StringConversionExtensions
    {
        /// <summary>
        /// Converts a fixed-length character array into a clean C# string, 
        /// stripping away trailing null termination formatting characters.
        /// </summary>
        /// <param name="chars">The raw source character array buffer.</param>
        /// <returns>A trimmed string, or an empty string if the character array is null.</returns>
        private static string ToCleanString(this char[] chars)
        {
            if (chars == null) return string.Empty;

            // Instantly create the string window frame over the buffer array
            string rawString = new string(chars);

            // Find the first instance of a null terminator byte to drop empty padding data
            int nullIndex = rawString.IndexOf('\0');

            if (nullIndex >= 0)
            {
                return rawString.Substring(0, nullIndex).Trim();
            }

            return rawString.Trim();
        }
    }
}
