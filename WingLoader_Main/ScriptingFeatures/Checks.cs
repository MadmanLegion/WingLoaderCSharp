namespace WingLoader.Scripting
{
    /// <summary>
    /// Provides utility evaluation algorithms and string matching operators to process real-time 
    /// unmanaged memory contents against game scripting rules.
    /// </summary>
    internal static class Checks
    {
        /// <summary>
        /// Defines the available string matching operations used to validate memory data.
        /// </summary>
        internal enum CheckTypes { match, startsWith, endsWith, contains, nothing, startsAndEndsWith, matchWithCallsign, matchWithSurname, matchWithFirstname, doubleContains };

        /// <summary>
        /// Parses a raw text configuration string into its corresponding strongly typed <see cref="CheckTypes"/> identifier.
        /// </summary>
        /// <param name="check">The string configuration keyword representing the validation type.</param>
        /// <returns>The matched <see cref="CheckTypes"/> value, or <see cref="CheckTypes.nothing"/> if unmapped.</returns>
        internal static CheckTypes getCheckType(string check)
        {
            switch (check.ToLower())
            {
                case "match": return CheckTypes.match;
                case "startswith": return CheckTypes.startsWith;
                case "endswith": return CheckTypes.endsWith;
                case "contains": return CheckTypes.contains;
                case "nothing": return CheckTypes.nothing;
                case "startsandendswith": return CheckTypes.startsAndEndsWith;
                case "matchwithcallsign": return CheckTypes.matchWithCallsign;
                case "matchwithsurname": return CheckTypes.matchWithSurname;
                case "matchwithfirstname": return CheckTypes.matchWithFirstname;
                case "doublecontains": return CheckTypes.doubleContains;
                default: return CheckTypes.nothing;
            }
        }

        /// <summary>
        /// Resolves a functional evaluation delegate matching the requested operational mode.
        /// </summary>
        /// <param name="checkType">The evaluation rule configuration flag.</param>
        /// <returns>A reusable <see cref="Func{T1, T2, TResult}"/> pointing to the matching string checker method.</returns>
        internal static Func<string, string, bool> getOperation(CheckTypes checkType)
        {

            switch (checkType)
            {
                case CheckTypes.match:
                    return match;
                case CheckTypes.startsWith:
                    return startsWith;
                case CheckTypes.endsWith:
                    return endsWith;
                case CheckTypes.contains:
                    return contains;
                case CheckTypes.nothing:
                    return nothing;
                case CheckTypes.startsAndEndsWith:
                    return startsAndEndsWith;
                case CheckTypes.matchWithCallsign:
                    return matchWithSubstitution;
                case CheckTypes.matchWithSurname:
                    return matchWithSubstitution;
                case CheckTypes.matchWithFirstname:
                    return matchWithSubstitution;
                case CheckTypes.doubleContains:
                    return doubleContains;
                default:
                    return nothing;
            }
        }

        /// <summary>
        /// Fallback evaluation rule that completely skips parsing validation steps.
        /// </summary>
        /// <returns>Always returns <c>false</c>.</returns>
        private static bool nothing(string memoryText, string ruleValue)
        {
            return false;
        }

        /// <summary>
        /// Validates if the rule value substring sequence occurs inside the sampled memory text.
        /// </summary>
        private static bool contains(string memoryText, string ruleValue)
        {
            return memoryText.Contains(ruleValue);
        }

        /// <summary>
        /// Compares memory text with a rule string after converting newline breaks into whitespace tokens.
        /// </summary>
        private static bool match(string memoryText, string ruleValue)
        {
            return memoryText.Replace(Environment.NewLine, " ") == ruleValue;
        }

        /// <summary>
        /// Validates if the sampled memory text string starts with the designated rule value sequence.
        /// </summary>
        private static bool startsWith(string memoryText, string ruleValue)
        {
            return memoryText.StartsWith(ruleValue);
        }

        /// <summary>
        /// Validates if the sampled memory text string finishes with the designated rule value sequence.
        /// </summary>
        private static bool endsWith(string memoryText, string ruleValue)
        {
            return memoryText.EndsWith(ruleValue);
        }

        /// <summary>
        /// Evaluates a rule string split by a hash mark token (<c>#</c>), checking if the text starts 
        /// with the first segment and ends with the second segment.
        /// </summary>
        private static bool startsAndEndsWith(string memoryText, string ruleValue)
        {
            string[] valueParts = ruleValue.Split("#");
            if (valueParts.Length < 2)
            {
                return false;
            }
            else if (memoryText.StartsWith(valueParts[0]) && memoryText.EndsWith(valueParts[1]))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Evaluates a pipe-delimited text segment (<c>|</c>) and injects dynamic context variables 
        /// back into placeholder characters (<c>$</c>) before performing a matching evaluation check.
        /// </summary>
        private static bool matchWithSubstitution(string memoryText, string ruleValue)
        {
            string[] memoryTexts = memoryText.Split('|');
            string ruleMessage = ruleValue.Replace("$", memoryTexts[1]);
            return memoryTexts[0] == ruleMessage;
        }

        /// <summary>
        /// Evaluates a rule string split by a hash mark token (<c>#</c>), checking if both text elements 
        /// exist anywhere within the sampled memory string.
        /// </summary>
        private static bool doubleContains(string memoryText, string ruleValue)
        {
            string[] ruleValues = ruleValue.Split("#");
            bool check1 = memoryText.Contains(ruleValues[0]);
            bool check2 = memoryText.Contains(ruleValues[1]);
            return check1 && check2;
        }
    }
}