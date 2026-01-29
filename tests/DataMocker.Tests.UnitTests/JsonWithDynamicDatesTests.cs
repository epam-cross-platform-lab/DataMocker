using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using DataMocker.Mock.DatesReplacing;
using DataMocker.SharedModels.DatesReplacing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataMocker.Tests.UnitTests
{
    [TestClass]
    public class JsonWithDynamicDatesTests
    {
        [TestMethod]
        public void JsonWithDynamicDates_ToJsonWithDynamicDates_ReplacesUtcDates()
        {
            // Arrange
            var jsonWithDinamicDates = new JsonWithDynamicDates(JsonSerializer.Serialize(DateTime.UtcNow));

            // Act
            var result = jsonWithDinamicDates.ToJsonWithDatesReplacements();

            // Assert
            var regex = new Regex($"{DateTimeReplacementConstants.UtcDateReplacement}{DateTimeReplacementConstants.TimespanPattern}");
            var matches = regex.Matches(result);
            Assert.AreEqual(1, matches.Count);
        }

        [TestMethod]
        public void JsonWithDynamicDates_ToJsonWithDatesReplacements_ReplacesDates()
        {
            // Arrange
            var jsonWithDinamicDates = new JsonWithDynamicDates(JsonSerializer.Serialize(DateTime.Now));

            // Act
            var result = jsonWithDinamicDates.ToJsonWithDatesReplacements();

            // Assert
            var regex = new Regex($"{DateTimeReplacementConstants.DateReplacement}{DateTimeReplacementConstants.TimespanPattern}");
            var matches = regex.Matches(result);
            Assert.AreEqual(1, matches.Count);
        }

        [TestMethod]
        public void JsonWithDynamicDates_ToJsonWithDatesReplacements_ReplacesUtcDateOffsets()
        {
            // Arrange
            var jsonWithDinamicDates = new JsonWithDynamicDates(JsonSerializer.Serialize(DateTimeOffset.UtcNow));

            // Act
            var result = jsonWithDinamicDates.ToJsonWithDatesReplacements();

            // Assert
            var regex = new Regex($"{DateTimeReplacementConstants.UtcDateOffsetReplacement}{DateTimeReplacementConstants.TimespanPattern}");
            var matches = regex.Matches(result);
            Assert.AreEqual(1, matches.Count);
        }
    }
}
