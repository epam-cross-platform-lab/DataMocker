// =========================================================================
// Copyright 2018 EPAM Systems, Inc.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataMocker.Mock
{
    /// <summary>Provides logic for url routing.</summary>
    public static class Routes
    {
        private const string FindAnyPattern = @"{\w+}";
        private const string FindIgnorePattern = @"{\w+\?}";
        private const string UTF16Slash = @"\/";
        private const string AnyPattern = @"\w+";
        private const string IgnorePattern = @"(\/\w+)?";

        private static readonly List<string> routes= new List<string>();

        /// <summary>Route given url and return roted url.</summary>
        /// <param name="routedUrl">Url for route.</param>
        /// <returns>routed url in type <see cref="T:System.String"/>.</returns>
        public static string RoutedNameByUrl(Uri routedUrl)
        {
            return  RouteRuleToResourceName(routes.FirstOrDefault(route => IsUrlRouted(route, routedUrl)), routedUrl); 
        }

        /// <summary>Add a new route.</summary>
        /// <param name="routedUrl">New route.</param>
        public static void AddRoute(string routedUrl)
        {
            routedUrl = EditUrl(routedUrl);
            if (!routes.Contains(routedUrl))
            {
                routes.Add(routedUrl);
            }
        }

        /// <summary>Remove route from routes.</summary>
        /// <param name="routedUrl">Romoving route.</param>
        public static void DeleteRoute(string routedUrl)
        {
            routedUrl = EditUrl(routedUrl);
            if (routes.Contains(routedUrl))
            {
                routes.Remove(routedUrl);
            }
        }

        /// <summary>Delete all routes from routes list.</summary>
        public static void Clear()
        {
            routes.Clear();
        }

        private static string EditUrl(string routedUrl)
        {
            var segments = routedUrl.Split('/');
            var ignoredSegments = new List<string>();
            var otherSegments = new List<string>();
            var c = 1;
            while (Regex.IsMatch(segments[segments.Length - c], FindIgnorePattern))
            {
                ignoredSegments.Insert(0, Regex.Replace(segments[segments.Length - c], FindIgnorePattern, IgnorePattern));
                c++;
            }

            for (var i = 0; i < segments.Length - c + 1; i++)
            {
                otherSegments.Add(Regex.Replace(segments[i], FindAnyPattern, AnyPattern));
            }
            return UTF16Slash + string.Join(UTF16Slash, otherSegments) + string.Join(string.Empty, ignoredSegments);
        }

        private static bool IsUrlRouted(string route, Uri url)
        {
            var path = url.AbsolutePath;
            path = RemoveLastPart(path, "/");
            route = RemoveLastPart(route, UTF16Slash);
            return Regex.Replace(path, route, string.Empty) == string.Empty;
        }

        private static string RouteRuleToResourceName(string route, Uri routedUrl)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                return null;
            }

            var segments = routedUrl.Segments;
            var routedSegments = new Uri(routedUrl.GetDomain() + route.Replace(IgnorePattern, string.Empty).Replace("\\", string.Empty)).Segments;
            var routePath = string.Empty;
            for (int i = 0; i < routedSegments.Length; i++)
            {
                if (segments[i].Trim('/') != routedSegments[i].Trim('/'))
                {
                    routePath += segments[i];
                }
            }
            if (routePath == string.Empty)
            {
                routePath = string.Join(string.Empty, routedSegments).Remove(0, 1);
            }

            return "_" + RemoveLastPart(routePath.Replace("/", "_"), "_");
        } 

        private static string RemoveLastPart(string source, string partToRemove)
        {
            var indexOfLastPart = source.LastIndexOf(partToRemove, StringComparison.Ordinal);
            return indexOfLastPart != source.Length - partToRemove.Length ? source : source.Remove(indexOfLastPart);
        }
    }
}