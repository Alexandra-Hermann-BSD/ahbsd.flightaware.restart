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
using System.Collections.Generic;
using System.Linq;
using ahbsd.lib.flight;

namespace ahbsd.flightaware.fr24
{
    /// <summary>
    /// Static class to convert from a string line to <see cref="LineType"/>
    /// and from a LineType to a single string.
    /// </summary>
    public static class ConvertLineType
    {
        /// <summary>
        /// Converts a line or single word to <see cref="LineType"/>.
        /// </summary>
        /// <param name="line">The line or single word</param>
        /// <returns>The converted LineType</returns>
        public static LineType FromString(string line)
        {
            LineType result = LineType.Unknown;
            IList<string> parts;

            if (!string.IsNullOrEmpty(line))
            {
                parts = line.Split(' ').ToList();

                if (parts.Count >= 1)
                {
                    result = parts[0].ToUpper() switch
                    {
                        "FR24" => LineType.Sender,
                        "RECEIVER:" => LineType.Receiver,
                        _ => LineType.Unknown,
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a <see cref="LineType"/> to a single Word.
        /// </summary>
        /// <param name="lineType">The LineType to convert</param>
        /// <returns>The single word</returns>
        public static string FromLineType(LineType lineType)
        {
            string result = lineType switch
            {
                LineType.Receiver => "Receiver:",
                LineType.Sender => "FR24",
                _ => string.Empty,
            };
            return result;
        }
    }
}
