using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("DataMocker.Tests.UnitTests")]
namespace DataMocker.Mock.DatesReplacing
{
    internal class JsonWithDatesReplacements
    {
        private string _json;

        internal JsonWithDatesReplacements(string json)
        {
            _json = json;
        }
        internal string ToJsonWithDynamicDates()
        {
            var result = _json;
            result = MoveBackForUtcDates(result);
            result = MoveBackForUtcDatetimeOffsets(result);
            result = MoveBackForCommonDates(result);
            return result;
        }

        private static string MoveBackForUtcDatetimeOffsets(string result)
        {
            var regex = new Regex($"\"{DateTimeReplacementConstants.UtcDateOffsetReplacement}{DateTimeReplacementConstants.TimespanPattern}\"");
            var matches = regex.Matches(result);
            foreach (Match match in matches)
            {
                var timeSpanString = match.Value.Trim('"').Replace(DateTimeReplacementConstants.UtcDateOffsetReplacement, string.Empty);
                var timespan = TimeSpan.Parse(timeSpanString);
                var replacement = JsonConvert.SerializeObject(DateTimeOffset.UtcNow - timespan);
                result = Regex.Replace(result, match.Value.Replace("+", @"\+"), replacement);
            }

            return result;
        }

        private static string MoveBackForCommonDates(string result)
        {
            var regex = new Regex($"\"{DateTimeReplacementConstants.DateReplacement}{DateTimeReplacementConstants.TimespanPattern}\"");
            var matches = regex.Matches(result);
            foreach (Match match in matches)
            {
                var timeSpanString = match.Value.Trim('"').Replace(DateTimeReplacementConstants.DateReplacement, string.Empty);
                var timeSpan = TimeSpan.Parse(timeSpanString);
                var replacement = JsonConvert.SerializeObject(DateTime.Now - timeSpan);
                result = Regex.Replace(result, match.Value.Replace("+", @"\+"), replacement);
            }

            return result;
        }

        private static string MoveBackForUtcDates(string result)
        {
            var regex = new Regex($"\"{DateTimeReplacementConstants.UtcDateReplacement}{DateTimeReplacementConstants.TimespanPattern}\"");
            var matches = regex.Matches(result);
            foreach (Match match in matches)
            {
                var timeSpanString = match.Value.Trim('"').Replace(DateTimeReplacementConstants.UtcDateReplacement, string.Empty);
                var timeSpan = TimeSpan.Parse(timeSpanString);
                var replacement = JsonConvert.SerializeObject(DateTime.UtcNow - timeSpan);
                result = Regex.Replace(result, match.Value.Replace("+", @"\+"), replacement);
            }

            return result;
        }
    }
}

