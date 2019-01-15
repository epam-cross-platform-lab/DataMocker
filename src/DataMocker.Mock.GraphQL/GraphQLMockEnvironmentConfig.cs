// =========================================================================
// Copyright 2019 EPAM Systems, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// =========================================================================

using DataMocker.SharedModels.GraphQL;

namespace DataMocker.Mock.GraphQL
{
    /// <summary>
    ///     graphQL mock environment config.
    /// </summary>
    public class GraphQLMockEnvironmentConfig : MockEnvironmentConfig
    {
        /// <summary>
        ///     Gets or sets the graphQL url.
        /// </summary>
        /// <value>The graphQL url.</value>
        public string GraphQLUrl { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DataMocker.Mock.GraphQL.GraphQLMockEnvironmentConfig"/> class.
        /// </summary>
        public GraphQLMockEnvironmentConfig()
        { 
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DataMocker.Mock.GraphQL.GraphQLMockEnvironmentConfig"/> class.
        /// </summary>
        /// <param name="environmentArgs">Environment arguments.</param>
        public GraphQLMockEnvironmentConfig(GraphQLEnvironmentArgs environmentArgs) : base(environmentArgs)
        {
            InitializeWithEnvironmentArgs(environmentArgs);
        }

        /// <summary>
        ///     Initialize with specified args.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public override void Initialize(string args)
        {
            base.Initialize(args);

            var environmentArguments = Newtonsoft.Json.JsonConvert.DeserializeObject<GraphQLEnvironmentArgs>(args);
            if (environmentArguments == null)
            {
                return;
            }
            InitializeWithEnvironmentArgs(environmentArguments);

        }

        /// <summary>
        ///     Initialize with specified environmentArgs.
        /// </summary>
        /// <param name="environmentArgs">Environment arguments.</param>
        protected void InitializeWithEnvironmentArgs (GraphQLEnvironmentArgs environmentArgs) 
        {
            GraphQLUrl = environmentArgs.GraphQLUrl;
        }
    }
}
