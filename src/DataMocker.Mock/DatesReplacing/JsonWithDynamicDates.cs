using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DataMocker.Mock.DatesReplacing
{
    internal class JsonWithDynamicDates
    {
        private string _json;

        internal JsonWithDynamicDates(string json)
        {
            _json = json;
        }

        internal string ToJsonWithDatesReplacements()
        {
            var result = _json;
            result = ReplaceUtcDates(result);
            result = ReplaceUtcDatetimeOffsets(result);
            result = ReplaceDates(result);

            return result;
        }

        private string ReplaceUtcDatetimeOffsets(string result)
        {
            var regex = new Regex(DateTimeReplacementConstants.UtcDateTimeOffsetPattern);
            var matches = regex.Matches(result);
            foreach (Match match in matches)
            {
                var replacingDate = DateTimeOffset.Parse(match.Value.Trim('"'), CultureInfo.InvariantCulture);
                var replacement = $"\"{DateTimeReplacementConstants.UtcDateOffsetReplacement}{DateTimeOffset.UtcNow - replacingDate}\"";
                result = Regex.Replace(result, match.Value.Replace("+", @"\+"), replacement);
            }

            return result;
        }

        private string ReplaceDates(string result)
        {
            var regex = new Regex(DateTimeReplacementConstants.DateTimePattern);
            var matches = regex.Matches(result);
            foreach (Match m in matches)
            {
                var replacingDate = DateTime.Parse(m.Value.Trim('"'), CultureInfo.InvariantCulture);
                var replacement = $"\"{DateTimeReplacementConstants.DateReplacement}{DateTime.Now - replacingDate}\"";
                result = Regex.Replace(result, m.Value.Replace("+", @"\+"), replacement);
            }

            return result;
        }

        private string ReplaceUtcDates(string result)
        {
            var regex = new Regex(DateTimeReplacementConstants.UtcDateTimePattern);
            var matches = regex.Matches(result);
            foreach (Match match in matches)
            {
                var replacingDate = DateTime.Parse(match.Value.Trim('"'), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
                var replacement = $"\"{DateTimeReplacementConstants.UtcDateReplacement}{DateTime.UtcNow - replacingDate}\"";
                result = Regex.Replace(result, match.Value, replacement);
            }

            return result;
        }
    }

}

