
namespace DataMocker.Mock.DatesReplacing
{
	internal class DateTimeReplacementConstants
	{
        internal const string UtcDateTimePattern = @"""\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d(\.\d+)?Z""";
        internal const string DateTimePattern = @"""\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d(.\d+)?([+-][0-2]\d:[0-5]\d)""";
        internal const string UtcDateTimeOffsetPattern = @"""\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d(\.\d+)([+-]00:00)""";
        internal const string UtcDateReplacement = "#@%UTCNOW";
        internal const string DateReplacement = "#@%NOW";
        internal const string UtcDateOffsetReplacement = "#@%OFFNOWUTC";

        internal const string TimespanPattern = @"(-)?(\d+\.)?((0?\d)|(1\d)|(2[0-3]))(:[0-5]\d){2}(\.\d+)?";
    }
}

