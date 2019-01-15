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

using System.Reflection;

namespace DataMocker.Mock.GraphQL
{
    /// <summary>
    ///     GraphQL mock handler initializer.
    /// </summary>
    public class GraphQLMockHandlerInitializer : MockHandlerInitializer
    {
        private readonly GraphQLMockEnvironmentConfig _appGraphQLEnvironmentConfig;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DataMocker.Mock.GraphQL.GraphQLMockHandlerInitializer"/> class.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <param name="assembly">Assembly.</param>
        public GraphQLMockHandlerInitializer(string args, Assembly assembly) 
            : this(ToEnvironmentConfig(args), assembly)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DataMocker.Mock.GraphQL.GraphQLMockHandlerInitializer"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="assembly">Assembly.</param>
        public GraphQLMockHandlerInitializer(GraphQLMockEnvironmentConfig config, Assembly assembly) : base(config, assembly)
        {
            _appGraphQLEnvironmentConfig = config;
        }

        /// <summary>
        ///     Creates new instance of GraphQLMockRequestBuilder.
        /// </summary>
        /// <returns>The <see cref="T:DataMocker.Mock.GraphQL.GraphQLMockRequestBuilder"/>.</returns>
        protected override MockRequestBuilder RequestBuilder()
        {
            return new GraphQLMockRequestBuilder(_appGraphQLEnvironmentConfig);
        }

        private static GraphQLMockEnvironmentConfig ToEnvironmentConfig(string args)
        {
            var config = new GraphQLMockEnvironmentConfig();
            config.Initialize(args);
            return config;
        }
    }
}
