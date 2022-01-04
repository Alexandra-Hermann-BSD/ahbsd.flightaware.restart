//
//  Copyright 2022  Alexandra Hermann – Beratung, Software, Design
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
namespace ahbsd.flightaware.piaware
{
    /// <summary>
    /// Static class to convert strings to <see cref="ConnectionTarget"/>s and vice versa.
    /// </summary>
    public static class ConvertConnectionTarget
    {
        /// <summary>
        /// Converts a given string to <see cref="ConnectionTarget"/>.
        /// </summary>
        /// <param name="target">The given string</param>
        /// <returns>The converted <see cref="ConnectionTarget"/></returns>
        public static ConnectionTarget FromString(string target)
        {
            if (!Enum.TryParse(target, out ConnectionTarget result))
            {
                string target2 = target.Trim().Replace(' ', '-').Replace('-', '_');
                if (!Enum.TryParse(target2, out result))
                {
                    result = ConnectionTarget.unknown;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a given <see cref="ConnectionTarget"/> to string.
        /// </summary>
        /// <param name="target">The given <see cref="ConnectionTarget"/></param>
        /// <returns>The converted string</returns>
        public static string FromConnectionTarget(ConnectionTarget target)
        {
            string result;

            switch (target)
            {
                case ConnectionTarget.ADS_B_receiver:
                    result = "ADS-B receiver";
                    break;
                case ConnectionTarget.FlightAware:
                    result = "FlightAware";
                    break;
                case ConnectionTarget.ES_connections:
                    result = "ES connections";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
    }
}
