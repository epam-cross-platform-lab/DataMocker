using System;
using System.Text.RegularExpressions;
using DataMocker.Mock.DatesReplacing;
using DataMocker.SharedModels.DatesReplacing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DataMocker.Tests.UnitTests
{
    [TestClass]
    public class JsonWithDatesReplacementsTests
    {
        [TestMethod]
        public void JsonWithDatesReplacement_ToJsonWithDynamicDates_MovesBackUtcDates()
        {
            // Arrange
            var timespan = TimeSpan.FromMinutes(-43);
            var s = $"\"{DateTimeReplacementConstants.UtcDateReplacement}{JsonConvert.SerializeObject(timespan).Trim('"')}\"";
            var jsonWithDatesReplacement = new JsonWithDatesReplacements(s);

            // Act
            var result = jsonWithDatesReplacement.ToJsonWithDynamicDates();

            // Assert
            var dateTime = JsonConvert.DeserializeObject<DateTime>(result);
            var regex = new Regex(DateTimeReplacementConstants.UtcDateTimePattern);
            var matches = regex.Matches(result);
            Assert.AreNotEqual(default(DateTime), dateTime);
            Assert.AreEqual(1, matches.Count);
            
        }

        [TestMethod]
        public void JsonWithDatesReplacement_ToJsonWithDynamicDates_MovesBackDates()
        {
            // Arrange
            var timespan = TimeSpan.FromMinutes(22);
            var s = $"\"{DateTimeReplacementConstants.DateReplacement}{JsonConvert.SerializeObject(timespan).Trim('"')}\"";
            var jsonWithDatesReplacement = new JsonWithDatesReplacements(s);

            // Act
            var result = jsonWithDatesReplacement.ToJsonWithDynamicDates();

            // Assert
            var regex = new Regex(DateTimeReplacementConstants.DateTimePattern);
            var matches = regex.Matches(result);
            var dateTime = JsonConvert.DeserializeObject<DateTime>(result);
            Assert.AreNotEqual(default(DateTime), dateTime);
            Assert.AreEqual(1, matches.Count);
        }

        [TestMethod]
        public void JsonWithDatesReplacement_ToJsonWithDynamicDates_MovesBackUtcDateOffsets()
        {
            // Arrange
            var timespan = TimeSpan.FromMinutes(0);
            var s = $"\"{DateTimeReplacementConstants.UtcDateOffsetReplacement}{JsonConvert.SerializeObject(timespan).Trim('"')}\"";
            var jsonWithDatesReplacement = new JsonWithDatesReplacements(s);

            // Act
            var result = jsonWithDatesReplacement.ToJsonWithDynamicDates();

            // Assert
            var regex = new Regex(DateTimeReplacementConstants.UtcDateTimeOffsetPattern);
            var matches = regex.Matches(result);
            var dateTime = JsonConvert.DeserializeObject<DateTimeOffset>(result);
            Assert.AreNotEqual(default(DateTimeOffset), dateTime);
            Assert.AreEqual(1, matches.Count);
        }
    }
}

