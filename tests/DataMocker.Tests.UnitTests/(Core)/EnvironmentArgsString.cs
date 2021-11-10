using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataMocker.Tests.UnitTests.Core
{
    public class EnvironmentArgsString
    {
        private readonly EnvironmentArgs environment;

        public EnvironmentArgsString(
            IList<string> testScenarios = null,
            string testName = null,
            string fileFormat = null,
            string language = null,
            IList<string> sharedFoldersList = null,
            string remoteUrl = null,
            int delay = 0
        ) : this(new EnvironmentArgs
                {
                    Delay = delay,
                    RemoteUrl = remoteUrl,
                    Language = language,
                    SharedFolderPath = sharedFoldersList,
                    TestName = testName,
                    TestScenario = testScenarios
                })
        {
        }

        public EnvironmentArgsString(EnvironmentArgs environment)
        {
            this.environment = environment;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(environment);
        }
    }
}

